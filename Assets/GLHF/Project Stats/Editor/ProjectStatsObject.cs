using System;
using UnityEngine;

public class ProjectStatsObject : ScriptableObject
{
    [SerializeField] [HideInInspector] private int _totalEditorEntries;
    [SerializeField] [HideInInspector] private int _totalProjectTime;
    [SerializeField] [HideInInspector] private int _longestSession;
    [SerializeField] [HideInInspector] private string _startDate;
    [SerializeField] [HideInInspector] private string _longestSessionDate;
    [SerializeField] [HideInInspector] private int _currentSessionTime;


    #region Public 

    public int TotalEditorEntries => _totalEditorEntries; 
    public int TotalTotalTime => _totalProjectTime; 
    public int LongestSession => _longestSession; 
    public string StartDate  => _startDate; 
    public string LongestSessionDate => _longestSessionDate; 
    public int CurrentSessionTime => _currentSessionTime; 

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

    public void RestartData()
    {
        _totalEditorEntries = 0;
        _totalProjectTime = 0;
        _longestSession = 0;
        _startDate = null;
        _currentSessionTime = 0;

        SetStartDate();
    }

    #endregion
}