using System;
using UnityEditor;
using UnityEngine;

namespace GLHF.ProjectStats
{
    [InitializeOnLoad]
    public class TrackActivity
    {
        private static bool isPlayMode;
        private static PersistentDataObject dataFile;
        private static ActivityData activityData;

        static TrackActivity()
        {
            EditorApplication.update += OnEditorUpdate;
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
            EditorApplication.quitting += OnEditorQuit;
        }


        #region Events

        private static void OnEditorLaunch()
        {
            activityData.OnEditorLaunch();
            activityData.ProjectEntries += 1;
        }

        private static void OnEditorUpdate()
        {
            if (dataFile == null)
            {
                dataFile = PersistentDataFileHandler.DataFile;

                if (dataFile != null)
                {
                    activityData = dataFile.ActivityStats;

                    FirstEditorUpdate();
                }

                return;
            }

            CountTime();
            dataFile.SetActivityStats(activityData);
        }

        private static void OnPlayModeStateChange(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    OnEditModeExit();
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    OnPlayModeEnter();
                    break;

                case PlayModeStateChange.ExitingPlayMode:
                    OnPlayModeExit();
                    break;
            }
        }

        private static void OnEditModeExit()
        {
            activityData.TotalPlayPressed += 1;
            activityData.EditModeExitTimeStamp = (int)EditorApplication.timeSinceStartup;
        }

        private static void OnPlayModeEnter()
        {
            isPlayMode = true;
        }

        private static void OnPlayModeExit()
        {
            isPlayMode = false;

            // In case of the user tapping the play button when having a tantrum
            if (activityData == null)
                return;

            var playTime = (int)Time.time;
            var timeNotEditing = (int)EditorApplication.timeSinceStartup - activityData.EditModeExitTimeStamp;

            activityData.TimeNotEditing += timeNotEditing;
            activityData.CurrentPlayTimeSession = 0;
            activityData.CurrentPlayTimeTotal += playTime;
        }

        private static void OnEditorQuit()
        {
            // Set remaining data:
            activityData.PastEditTime += activityData.CurrentEditTime;
            activityData.PastPlayTime += activityData.CurrentPlayTimeTotal;
            activityData.ProjectLastDate = DateTime.Now.Date.ToShortDateString();

            // Rest some of the data:
            activityData.FirstDataCollectionTimeStamp = 0;
            activityData.CurrentPlayTimeTotal = 0;
            activityData.TimeNotEditing = 0;

            PersistentDataFileHandler.SaveDataFile();
        }

        #endregion


        #region Functions

        private static void CountTime()
        {
            if (isPlayMode)
            {
                var playTime = (int)Time.time;
                activityData.CurrentPlayTimeSession = playTime;
            }
            else
            {
                var timeSinceStartup = (int)EditorApplication.timeSinceStartup;
                activityData.TimeSinceStartup = timeSinceStartup;
            }

            var currentEditTime = activityData.CurrentEditTime;
            var currentPlayTime = activityData.CurrentPlayTime;
            var currentTotalTime = currentEditTime + currentPlayTime;

            if (currentTotalTime > activityData.LongestSession)
            {
                activityData.LongestSession = currentTotalTime;
                activityData.LongestSessionDate = DateTime.Now.ToShortDateString();
            }
        }

        private static void FirstEditorUpdate()
        {
            var startDate = activityData.ProjectStartDate;
            var isFirstUpdate = String.IsNullOrEmpty(startDate);

            if (isFirstUpdate)
            {
                activityData.FirstDataCollectionTimeStamp = (int)EditorApplication.timeSinceStartup;
                activityData.ProjectStartDate = DateTime.Now.ToShortDateString();
                activityData.ProjectLastDate = DateTime.Now.ToShortDateString();
            }

            var isFirstUpdateSinceEditorLaunch = activityData.OnEditorLaunchHack == false;

            if (isFirstUpdateSinceEditorLaunch)
            {
                OnEditorLaunch();
            }
        }

        #endregion
    }
}