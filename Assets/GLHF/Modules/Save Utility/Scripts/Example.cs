using UnityEngine;

namespace GLHF.SaveUtility
{
    [ExecuteAlways]
    public class Example : MonoBehaviour
    {
        [Header("Works In Edit Mode")]
        [SerializeField] private bool simulateGameSave;
        [SerializeField] private bool simulateGameLoad;

        [Header("Data")]
        [SerializeField] GameData gameData;


        private void Update()
        {
            if (simulateGameSave)
            {
                SaveUtility.SaveData(gameData);
                simulateGameSave = false;
            }

            if (simulateGameLoad)
            {
                var loadedGameData = SaveUtility.LoadData();
                
                if(loadedGameData != null)
                {
                    gameData = loadedGameData;
                }

                simulateGameLoad = false;
            }
        }
    }
}