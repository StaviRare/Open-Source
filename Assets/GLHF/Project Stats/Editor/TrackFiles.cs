using System.IO;
using UnityEditor;
using UnityEngine;
using System;

namespace GLHF.ProjectStats
{
    public static class TrackFiles
    {
        private static PersistentDataObject dataFile;
        private static FileData fileStats;

        public static void GetProjectFileData(UnityEngine.Object customFolder = null)
        {
            dataFile = PersistentDataFileHandler.DataFile;

            if (dataFile == null)
            {
                Debug.Log("Could not find data file");
                return;
            }

            var dataPath = Application.dataPath;

            if (customFolder != null)
            {
                var customFolderPath = AssetDatabase.GetAssetPath(customFolder);

                if (Directory.Exists(customFolderPath))
                {
                    dataPath = customFolderPath;
                }
                else
                {
                    Debug.Log("Directory does not exists!");
                }
            }

            fileStats = new FileData()
            {
                LastDataGatherDate = DateTime.Now.ToString()
            };

            GetFolderData(dataPath);
            dataFile.SetFileStats(fileStats);
        }

        private static void GetFolderData(string folderPath)
        {
            var folderPaths = Directory.GetDirectories(folderPath);
            var filePaths = Directory.GetFiles(folderPath);

            foreach (var folder in folderPaths)
            {
                GetFolderData(folder);
                fileStats.FolderCount += 1;
            }

            foreach (var file in filePaths)
            {
                GetFileData(file);
            }
        }

        private static void GetFileData(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath);

            switch (fileExtension)
            {
                case ".cs":
                    ReadScriptData(filePath);
                    break;

                case ".prefab":
                    ReadPrefabData(filePath);
                    break;

                case ".mat":
                    ReadMaterialData(filePath);
                    break;
            }
        }


        #region File Type Read Functions

        private static void ReadScriptData(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var lineCount = File.ReadAllLines(filePath).Length;
            var wordCount = File.ReadAllText(filePath).Split().Length;

            fileStats.ScriptFileCount += 1;
            fileStats.ScriptLineCount += lineCount;
            fileStats.ScriptWordCount += wordCount;
        }

        private static void ReadPrefabData(string filePath)
        {
            fileStats.PrefabCount += 1;
        }

        private static void ReadMaterialData(string filePath)
        {
            fileStats.MaterialCount += 1;
        }

        #endregion
    }
}