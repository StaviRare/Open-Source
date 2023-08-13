using UnityEngine;
using System.IO;
using UnityEditor;

namespace GLHF.ProjectStats
{
    [InitializeOnLoad]
    public static class PersistentDataFileHandler
    {
        public static PersistentDataObject DataFile { private set; get; }

        private const string dataFileName = "Data";
        private const string dataFilePath = "Assets/GLHF/Project Stats/Resources/";
        private const string deleteDataDialogTitle = "Project Stats Reset";
        private const string deleteDataDialogContent = "Are you sure you want to delete all project stats?";
        private const string deleteDataDialogAnswerNo = "No";
        private const string deleteDataDialogAnswerYes = "Yes";

        static PersistentDataFileHandler()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        public static void SaveDataFile()
        {
            EditorUtility.SetDirty(DataFile);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void DeleteDataFile()
        {
            var dataDeletionDeclined
                = !EditorUtility.DisplayDialog(deleteDataDialogTitle, deleteDataDialogContent, deleteDataDialogAnswerYes, deleteDataDialogAnswerNo);

            if (dataDeletionDeclined)
                return;

            AssetDatabase.DeleteAsset(dataFilePath + dataFileName + ".asset");
        }

        private static void OnEditorUpdate()
        {
            if (DataFile != null)
                return;

            GetDataFile();
        }

        private static void GetDataFile()
        {
            DataFile = Resources.Load(dataFileName) as PersistentDataObject;

            if (DataFile != null)
                return;

            var directoryDoesNotExists = !Directory.Exists(dataFilePath);

            if (directoryDoesNotExists)
            {
                Directory.CreateDirectory(dataFilePath);
            }

            DataFile = ScriptableObject.CreateInstance<PersistentDataObject>();
            AssetDatabase.CreateAsset(DataFile, dataFilePath + dataFileName + ".asset");
            SaveDataFile();
        }
    }
}