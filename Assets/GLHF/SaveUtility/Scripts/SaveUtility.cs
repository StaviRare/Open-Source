using System;
using System.IO;
using UnityEngine;

namespace GLHF.SaveUtility
{
    public static class SaveUtility
    {
        private static readonly string fileName = "/SaveFile.json";
        private static readonly string directoryPath = Application.persistentDataPath + "/Data";
        private static readonly string fullFilePath = directoryPath + fileName;

        public static GameData LoadData()
        {
            try
            {
                var encryptedJson = File.ReadAllText(fullFilePath);
                var jSon = CryptoUtility.DecryptString(encryptedJson);
                var gameData = JsonUtility.FromJson<GameData>(jSon);
                Debug.Log("Load operation was successful!");

                return gameData;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Load operation failed!");
                Debug.LogWarning(e);

                return null;
            }
        }

        public static void SaveData(GameData gameData)
        {
            try
            {
                var jSon = JsonUtility.ToJson(gameData);
                var encryptedJson = CryptoUtility.EncryptString(jSon);

                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(fullFilePath, encryptedJson);

                Debug.Log("Save operation was successful!");
                Debug.Log(fullFilePath);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Save operation failed!");
                Debug.LogWarning(e);
            }
        }
    }
}