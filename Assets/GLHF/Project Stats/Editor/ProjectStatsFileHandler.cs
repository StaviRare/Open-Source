using UnityEngine;
using System.IO;
using UnityEditor;

public static class ProjectStatsFileHandler
{
    public static ProjectStatsObject DataFile { private set; get; }

    private const string _dataFileName = "Data";
    private const string _dataFilePath = "Assets/GLHF/Project Stats/Resources/";

    static ProjectStatsFileHandler()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnEditorUpdate()
    {
        if (DataFile != null)
            return;

        GetDataFile();
    }

    public static void GetDataFile()
    {
        DataFile = Resources.Load(_dataFileName) as ProjectStatsObject;

        if (DataFile != null)
            return;

        var directoryDoesNotExists = !Directory.Exists(_dataFilePath);

        if (directoryDoesNotExists)
        {
            Directory.CreateDirectory(_dataFilePath);
        }

        DataFile = ScriptableObject.CreateInstance<ProjectStatsObject>();
        AssetDatabase.CreateAsset(DataFile, _dataFilePath + _dataFileName + ".asset");
        AssetDatabase.SaveAssets();
    }
}
