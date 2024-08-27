using System;
using UnityEngine;
using NaughtyAttributes;
using ScriptableObjects;
using Unity.VisualScripting;


public class ScoreManager 
{
    // Score
    private float _globalscore = 0;
    private float _milestoneScore = 0;
    
    // Action value
    private float _actionValue = 0;
    private float _avForNextMileStone = 0;
    
    // MileStone 
    private int _mileStoneIndex = 1;
    
    // Data
    private SO_ScoreManager _data;
    
    private string DataPath => "ScriptableObject/SO_ScoreManager";
    
    public float GetGlobalScore() => _globalscore;
    public int GetMileStoneIndex() => _mileStoneIndex;
    public float GetCoefDifficulty() => Mathf.Pow(_data._baseCoefficientDiffiCulty, _mileStoneIndex);
    public void ResetActionValue() => _actionValue = 0;
    public void ResetMileStoneIndex() => _mileStoneIndex = 0;
    public void UpdateAvForNextMileStone() =>_avForNextMileStone += _avForNextMileStone * _data._avNextMilestoneCoef;
    public void UpdateMileStoneIndex() => _mileStoneIndex++;
    
    public ScoreManager()
    {
        // Subscribe to the event
        GameManager.Instance.ActionManager.PlayerMove += UpdateActionValue;
        GameManager.Instance.ActionManager.MilestoneEvent += UpdateMileStoneScore;
        
        if (_data == null) 
            _data = Resources.Load<SO_ScoreManager>(DataPath);
        
        if (GameManager.Instance.NewGame) {
            _avForNextMileStone = _data._avForNextMileStoneBase;
        } else {
            // TODO : Load Score and data from save
        }
    }
    
    ~ScoreManager()
    {
        // Unsubscribe to the event
        if (GameManager.Instance == null) return;
        GameManager.Instance.ActionManager.PlayerMove -= UpdateActionValue;
        GameManager.Instance.ActionManager.MilestoneEvent -= UpdateMileStoneScore;
    }
    
    // Update the global score
    public void UpdateScore()
    {
        _globalscore = _actionValue + _milestoneScore;
        GameManager.Instance.ActionManager.InvokeScoreUpdate(_globalscore);
    }
    
    // Update the action value 
    public void UpdateActionValue(float playerPosZ, float playerPosLastFrameZ, bool isOnRoad)
    {
        _actionValue += (playerPosZ - playerPosLastFrameZ) * (isOnRoad ? _data._avRoadScoreMultiplier : 1);
        if (_actionValue >= _avForNextMileStone) {
            GameManager.Instance.ActionManager.InvokeAvForNextMilestoneReached(_avForNextMileStone);
            UpdateAvForNextMileStone();
            UpdateMileStoneIndex();
        }
        UpdateScore();
    }
    
    // Update the milestone score
    public void UpdateMileStoneScore(int luggageCount)
    {
        _milestoneScore += _data._milestoneScoreCoef * luggageCount * _mileStoneIndex;
        Debug.LogFormat("Update Milestone Score : {0}", _milestoneScore);
        UpdateScore();
    }
}
