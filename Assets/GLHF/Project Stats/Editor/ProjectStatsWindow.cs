using System;
using UnityEngine;
using UnityEditor;
using GLHF.ProjectStats;

public class ProjectStatsWindow : EditorWindow
{
    private FileData fileStats;
    private ActivityData activityStats;
    private PersistentDataObject persistentDataObject;
    private UnityEngine.Object customFolder = null;
    private int toolbarIndex = 0;
    private bool missingReferences = true;

    private const string dateFormat = "dd/MM/yyyy";
    private readonly string[] toolbarNames = { "Activity", "Files", "Settings" };
    private static readonly Vector2 windowSize = new Vector2(350, 350);

    [MenuItem("GLHF/Project Stats #F1", false, 0)]
    static void Init()
    {
        var projectStatsWindow = (ProjectStatsWindow)GetWindow(typeof(ProjectStatsWindow));
        projectStatsWindow.maxSize = windowSize;
        projectStatsWindow.minSize = projectStatsWindow.maxSize;

        projectStatsWindow.Show();
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GetReferneces();

        if (missingReferences)
            return;

        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarNames);

        switch (toolbarIndex)
        {
            case 0:
                DisplayProjectData();
                DisplayLongestData();
                DisplayMiscActivity();
                //DisplayDebug();
                break;

            case 1:
                DisplayFileCategory();
                break;

            case 2:
                DisplaySettingsCategory();
                break;
        }
    }


    #region GUI Display Activity

    private void DisplayProjectData()
    {
        var totalProjectTime = CalculateClockTime(activityStats.ProjectTime);
        var totalEditTime = CalculateClockTime(activityStats.TotalEditTime);
        var totalPlayTime = CalculateClockTime(activityStats.TotalPlayTime);

        EditorGUILayout.BeginVertical("TextArea");
        SetGUIHeaderBold("Project Duration");
        SetGUILabel("Total Time", totalProjectTime);
        SetGUILabel("Total Edit Mode", totalEditTime);
        SetGUILabel("Total Play Mode", totalPlayTime);
        EditorGUILayout.EndVertical();
    }

    private void DisplayLongestData()
    {
        var longestSession = CalculateClockTime(activityStats.LongestSession);
        var currentEntryTime = CalculateClockTime(activityStats.CurrentEntryTime);
        var longestSessionDate = DateTime.Parse(activityStats.LongestSessionDate).ToString(dateFormat);

        EditorGUILayout.BeginVertical("TextArea");
        SetGUIHeaderBold("Longest Duration");
        SetGUILabel("Current Entry", currentEntryTime);
        SetGUILabel("Longest Entry", longestSession);
        SetGUILabel("Longest Entry Date", longestSessionDate);
        EditorGUILayout.EndVertical();
    }

    private void DisplayMiscActivity()
    {
        var todayDate = DateTime.Now.Date;
        var lastSessionDate = DateTime.Parse(activityStats.ProjectLastDate);
        var dayPassedNumber = (todayDate - lastSessionDate).Days;
        var firstDateTitle = DateTime.Parse(activityStats.ProjectStartDate).ToString(dateFormat);
        var lastDateTitle = lastSessionDate.ToString(dateFormat) + " (" + dayPassedNumber + " days)";

        EditorGUILayout.BeginVertical("TextArea");
        SetGUIHeaderBold("Misc");
        SetGUILabel("Project Enteries", activityStats.ProjectEntries.ToString());
        SetGUILabel("Play Button Pressed", activityStats.TotalPlayPressed.ToString());
        SetGUILabel("First Project Entry Date", firstDateTitle);
        SetGUILabel("Last Project Entry Date", lastDateTitle);
        EditorGUILayout.EndVertical();
    }

    private void DisplayDebug()
    {
        EditorGUILayout.Space();

        var currentEditTime = CalculateClockTime(activityStats.CurrentEditTime);
        var currentPlayTime = CalculateClockTime(activityStats.CurrentPlayTime);

        SetGUILabel("Current In Edit Mode", currentEditTime);
        SetGUILabel("Current In Play Mode", currentPlayTime);
    }

    #endregion


    #region GUI Display File Data

    private void DisplayFileCategory()
    {
        EditorGUILayout.LabelField("Custom Folder:");
        customFolder = EditorGUILayout.ObjectField(customFolder, typeof(DefaultAsset), false);
        EditorGUILayout.HelpBox("Null reference will be considered as default (Assets folder)", MessageType.None);

        var lastDataGatherDate = fileStats.LastDataGatherDate;
        var dataHasBeenAlreadyGathered = !String.IsNullOrEmpty(lastDataGatherDate);
        var lastUpdatedTitle = dataHasBeenAlreadyGathered ? lastDataGatherDate : "Never";
        var collectDataButtonTitle = "Collect Data";

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox("Press \"" + collectDataButtonTitle + "\" to gather the latest file data", MessageType.Warning);
        EditorGUILayout.HelpBox("Last Update: " + lastUpdatedTitle, MessageType.Info);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button(collectDataButtonTitle, GUILayout.Height(40)))
        {
            TrackFiles.GetProjectFileData(customFolder);
        }

        DisplayScriptData();
        DisplayTotalAssetData();
    }

    private void DisplayScriptData()
    {
        var totalScriptLines = fileStats.ScriptLineCount.ToString();
        var totalScriptWords = fileStats.ScriptWordCount.ToString();
        var totalScriptNumber = fileStats.ScriptFileCount.ToString();

        EditorGUILayout.BeginVertical("TextArea");
        SetGUIHeaderBold("Scripts");
        SetGUILabel("Total Script Lines", totalScriptLines);
        SetGUILabel("Total Script Words", totalScriptWords);
        SetGUILabel("Total Script Count", totalScriptNumber);
        EditorGUILayout.EndVertical();
    }

    private void DisplayTotalAssetData()
    {
        var totalFolders = fileStats.FolderCount.ToString();
        var totalPrefabs = fileStats.PrefabCount.ToString();
        var totalMaterials = fileStats.MaterialCount.ToString();

        EditorGUILayout.BeginVertical("TextArea");
        SetGUIHeaderBold("Assets");
        SetGUILabel("Total Folders", totalFolders);
        SetGUILabel("Total Prefabs", totalPrefabs);
        SetGUILabel("Total Materials", totalMaterials);
        EditorGUILayout.EndVertical();
    }

    #endregion


    #region GUI Display Settings

    private void DisplaySettingsCategory()
    {
        EditorGUILayout.HelpBox("Warning! This will delete all of your project stats!", MessageType.Warning);

        if (GUILayout.Button("Restart All Data", GUILayout.Height(40)))
        {
            PersistentDataFileHandler.DeleteDataFile();
        }
    }

    #endregion


    #region Misc

    private void SetGUILabel(string header, string content)
    {
        EditorGUILayout.LabelField(header + ": " + content);
    }

    private void SetGUIHeaderBold(string header)
    {
        EditorGUILayout.LabelField(header + ":", EditorStyles.boldLabel);
    }

    private string CalculateClockTime(float time)
    {
        var seconds = (int)(time % 60);
        time /= 60;
        var minutes = (int)(time % 60);
        time /= 60;
        var hours = (int)(time);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    private void GetReferneces()
    {
        persistentDataObject = PersistentDataFileHandler.DataFile;
        missingReferences = persistentDataObject == null;

        if (missingReferences)
            return;

        fileStats = persistentDataObject.FileStats;
        activityStats = persistentDataObject.ActivityStats;
    }

    #endregion
}