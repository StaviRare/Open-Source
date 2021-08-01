using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "ProjectStats", menuName = "General/ProjectStats", order = 1)]
public class ProjectStatsObject : ScriptableObject
{
    [SerializeField] [HideInInspector] private int _totalEditorEntries;
    [SerializeField] [HideInInspector] private int _totalProjectTime;
    [SerializeField] [HideInInspector] private int _longestSession;
    [SerializeField] [HideInInspector] private string _startDate;
    [SerializeField] [HideInInspector] private string _longestSessionDate;
    [SerializeField] [HideInInspector] private int _currentSessionTime;


    #region Public 

    public int TotalEditorEntries { get => _totalEditorEntries; }
    public int TotalTotalTime { get => _totalProjectTime; }
    public int LongestSession { get => _longestSession; }
    public string StartDate { get => _startDate; }
    public string LongestSessionDate { get => _longestSessionDate; }
    public int CurrentSessionTime { get => _currentSessionTime; }

    public void SetTotalTime(int i)
    {
        _totalProjectTime += i;
    }

    public void SetCurrentTime(int i)
    {
        _currentSessionTime = i;

        if(_currentSessionTime > _longestSession)
        {
            _longestSession = _currentSessionTime;
            _longestSessionDate = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    public void SetStartDate()
    {
        if (String.IsNullOrEmpty(_startDate))
        {
            _startDate = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    public void SetEditorSessionEnd()
    {
        _totalEditorEntries += 1;
    }

    #endregion
}