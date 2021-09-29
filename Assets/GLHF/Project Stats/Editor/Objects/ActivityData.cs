using System;

namespace GLHF.ProjectStats
{
    [Serializable]
    public class ActivityData
    {
        // Simple data
        public int ProjectEntries;
        public string ProjectStartDate;
        public string ProjectLastDate;
        public int TotalPlayPressed;

        // Easy access data
        public int ProjectTime => TotalEditTime + TotalPlayTime;
        public int TotalEditTime => PastEditTime + CurrentEditTime;
        public int TotalPlayTime => PastPlayTime + CurrentPlayTime;
        public int CurrentEntryTime => CurrentEditTime + CurrentPlayTime;
        public int CurrentEditTime => TimeSinceStartup - TimeNotEditing - FirstDataCollectionTimeStamp;
        public int CurrentPlayTime => CurrentPlayTimeTotal + CurrentPlayTimeSession;

        // Past data
        public int PastEditTime;
        public int PastPlayTime;

        // Current play
        public int CurrentPlayTimeTotal;
        public int CurrentPlayTimeSession;

        // Current edit
        public int TimeNotEditing;
        public int EditModeExitTimeStamp;
        public int TimeSinceStartup;
        public int FirstDataCollectionTimeStamp;

        // Record data 
        public int LongestSession;
        public string LongestSessionDate;


        #region OnEditor Launch Hack

        public bool OnEditorLaunchHack => onEditorLaunchHack;

        private bool onEditorLaunchHack = false;

        public void OnEditorLaunch()
        {
            onEditorLaunchHack = true;
        }

        #endregion
    }
}