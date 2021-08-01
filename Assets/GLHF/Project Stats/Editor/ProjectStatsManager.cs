using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
class ProjectStatsManager
{
    public static ProjectStatsObject DataFile { private set; get; }

    private static int _timeSinceStartup;

    static ProjectStatsManager()
    {
        EditorApplication.update += OnEditorUpdate;
        EditorApplication.quitting += OnEditorQuit;
    }

    private static void OnEditorUpdate()
    {
        GetDataFile();

        if (DataFile == null)
            return;

        GetTimeSinceStartup();
    }

    private static void OnEditorQuit()
    {
        DataFile.SetEditorSessionEnd();
        DataFile.SetTotalTime(_timeSinceStartup);
        DataFile.SetCurrentTime(_timeSinceStartup);

        EditorUtility.SetDirty(DataFile);
        AssetDatabase.SaveAssets();
    }

    private static void GetTimeSinceStartup()
    {
        _timeSinceStartup = (int) EditorApplication.timeSinceStartup;
        DataFile.SetCurrentTime(_timeSinceStartup);
    }

    private static void GetDataFile()
    {
        if (DataFile != null)
            return;

        DataFile = Resources.Load("Data") as ProjectStatsObject;
        FirstEditorUpdate();
    }

    private static void FirstEditorUpdate()
    {
        DataFile.SetStartDate();
    }
}