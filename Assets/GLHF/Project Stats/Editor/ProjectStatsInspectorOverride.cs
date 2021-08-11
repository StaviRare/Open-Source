using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProjectStatsObject))]
public class ProjectStatsInspectorOverride : Editor
{
    private const int buttonSize = 40;
    private const string buttonTitle = "Restart Data";
    private const string dialogTitle = "Project Stats Reset";
    private const string dialogContent = "Are you sure you want to delete all project stats?";
    private const string dialogAnswerYes = "Yes";
    private const string dialogAnswerNo = "No";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button(buttonTitle, GUILayout.Height(buttonSize)))
        {
            var deleteData = EditorUtility.DisplayDialog(dialogTitle, dialogContent, dialogAnswerYes, dialogAnswerNo);

            if(deleteData)
            {
                var script = (ProjectStatsObject)target;
                script.RestartData();
            }
        }
    }
}
