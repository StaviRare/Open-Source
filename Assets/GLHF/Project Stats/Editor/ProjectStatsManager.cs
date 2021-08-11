using UnityEditor;

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
        if (DataFile == null)
        {
            DataFile = ProjectStatsFileHandler.DataFile;

            if (DataFile != null)
            {
                FirstEditorUpdate();
            }

            return;
        }
            
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

    private static void FirstEditorUpdate()
    {
        DataFile.SetStartDate();
    }
}