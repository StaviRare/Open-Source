using UnityEngine;
using UnityEditor;

public class ProjectStatsWindow : EditorWindow
{
    private ProjectStatsObject _dataFile;

    [MenuItem("GLHF/Project Stats #F1", false, 0)]
    static void Init()
    {
        ProjectStatsWindow window = (ProjectStatsWindow)EditorWindow.GetWindow(typeof(ProjectStatsWindow));
        window.Show();
    }

    private void Update()
    {
        _dataFile = ProjectStatsFileHandler.DataFile;

        Repaint();
    }

    private void OnGUI()
    {
        if (_dataFile == null)
            return;
        
        // Not implemented yet!
        // isPlay total time
        // Editor total time
        // Reset data in settings?
        //EditorGUILayout.LabelField("Settings:");
        //GUILayout.Toggle(true, "Date Format");


        var timer = CalculateClockTime(_dataFile.TotalTotalTime + _dataFile.CurrentSessionTime);
        var longest = CalculateClockTime(_dataFile.LongestSession);

        EditorGUILayout.LabelField("Project:");
        GUILayout.Label("Total Time: " + timer, EditorStyles.boldLabel);
        GUILayout.Label("Start Date: " + _dataFile.StartDate, EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Session:");
        GUILayout.Label("Total Sessions: " + _dataFile.TotalEditorEntries, EditorStyles.boldLabel);
        GUILayout.Label("Longest Session Time: " + longest, EditorStyles.boldLabel);
        GUILayout.Label("Longest Session Date: " + _dataFile.LongestSessionDate, EditorStyles.boldLabel);
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
}