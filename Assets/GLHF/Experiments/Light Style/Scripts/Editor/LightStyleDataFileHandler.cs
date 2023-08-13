using System.IO;
using UnityEditor;
using UnityEngine;

namespace GLHF.LightStyles
{
    public static class LightStyleDataFileHandler
    {
        public static LightStyleData DataFile { private set; get; }

        private const string _dataFileName = "LightStyleData";
        private const string _dataFilePath = "Assets/GLHF/Light Style/Resources/";

        static LightStyleDataFileHandler()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private static void OnEditorUpdate()
        {
            if (DataFile != null)
                return;

            GetDataFile();
        }

        private static void GetDataFile()
        {
            DataFile = Resources.Load(_dataFileName) as LightStyleData;

            var dataFileIsNotMissing = DataFile != null;

            if (dataFileIsNotMissing)
                return;

            var directoryDoesNotExists = !Directory.Exists(_dataFilePath);

            if (directoryDoesNotExists)
            {
                Directory.CreateDirectory(_dataFilePath);
            }

            DataFile = ScriptableObject.CreateInstance<LightStyleData>();
            AssetDatabase.CreateAsset(DataFile, _dataFilePath + _dataFileName + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}