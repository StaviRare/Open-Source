using UnityEngine;
using UnityEditor;

public class ProjectStatsWindow : EditorWindow
{
    private ProjectStatsObject dataFile;

    [MenuItem("GLHF/Project Stats #F1", false, 0)]
    static void Init()
    {
        ProjectStatsWindow window = (ProjectStatsWindow)EditorWindow.GetWindow(typeof(ProjectStatsWindow));
        window.Show();
    }

    private void OnEnable()
    {
        if (dataFile != null)
            return;

        dataFile = ProjectStatsManager.DataFile;

        if (dataFile == null)
            Close();
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        if (dataFile == null)
            return;

        var timer = CalculateClockTime(dataFile.TotalTotalTime + dataFile.CurrentSessionTime);
        var longest = CalculateClockTime(dataFile.LongestSession);

        EditorGUILayout.LabelField("Project:");
        GUILayout.Label("Total Time: " + timer, EditorStyles.boldLabel);
        GUILayout.Label("Start Date: " + dataFile.StartDate, EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Session:");
        GUILayout.Label("Total Sessions: " + dataFile.TotalEditorEntries, EditorStyles.boldLabel);
        GUILayout.Label("Longest Session Time: " + longest, EditorStyles.boldLabel);
        GUILayout.Label("Longest Session Date: " + dataFile.LongestSessionDate, EditorStyles.boldLabel);
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