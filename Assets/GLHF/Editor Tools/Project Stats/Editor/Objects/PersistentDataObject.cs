using System;
using UnityEngine;

namespace GLHF.ProjectStats
{
    [Serializable]
    public class PersistentDataObject : ScriptableObject
    {
        public FileData FileStats => fileStats;
        public ActivityData ActivityStats => activityStats;

        [HideInInspector] [SerializeField] private FileData fileStats = new FileData();
        [HideInInspector] [SerializeField] private ActivityData activityStats = new ActivityData();


        public void SetFileStats(FileData data)
        {
            fileStats = data;
        }

        public void SetActivityStats(ActivityData data)
        {
            activityStats = data;
        }
    }
}