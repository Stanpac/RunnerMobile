using System;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;


public class ScoreManager 
{
    // score of the player for the current game
    private float _globalscore = 0;
    
    // score gain by the player on the milestone
    private float _milestoneScore = 0;
    
    // Action value
    private float _actionValue = 0;
    private float _avForNextMileStone = 0;
    private float _avForNextMileStoneBase = 1000;
    private float _avRoadScoreMultiplier = 1.5f;
    
    // MileStone 
    private int _mileStoneIndex = 1;
    private float _avNextMilestoneCoef = 1.1f;
    private float _milestoneScoreCoef = 2f;
    
    // Coefficient of difficulty
    private float _baseCoefficientDiffiCulty = 2f;
    
    
    public float GetGlobalScore() => _globalscore;
    public int GetMileStoneIndex() => _mileStoneIndex;
    public float GetCoefDifficulty() => Mathf.Pow(_baseCoefficientDiffiCulty, _mileStoneIndex)  ;
    public void ResetActionValue() => _actionValue = 0;
    public void ResetMileStoneIndex() => _mileStoneIndex = 0;
    public void UpdateAvForNextMileStone() =>_avForNextMileStone += _avForNextMileStone * _avNextMilestoneCoef;
    public void UpdateMileStoneIndex() => _mileStoneIndex++;
    
    // TODO : Event call a each milestone for update MilestoneScore
    
    public void UpdateScore()
    {
        // Score = AV + milestoneScore
        _globalscore = _actionValue + _milestoneScore;
        GameManager.Instance.ActionManager.InvokeScoreUpdate(_globalscore);
    }
    
    public void UpdateActionValue(float playerPosZ, float playerPosLastFrameZ, bool isOnRoad)
    {
        _actionValue += (playerPosZ - playerPosLastFrameZ) * (isOnRoad ? _avRoadScoreMultiplier : 1);
        if (_actionValue >= _avForNextMileStone) {
            GameManager.Instance.ActionManager.InvokeAvForNextMilestoneReached(_avForNextMileStone);
            UpdateAvForNextMileStone();
            UpdateMileStoneIndex();
        }
        UpdateScore();
    }
    
    public void UpdateMileStoneScore(int luggageCount)
    {
        _milestoneScore += _milestoneScoreCoef * luggageCount * _mileStoneIndex;
        UpdateScore();
    }
    
    public ScoreManager()
    {
        // Subscribe to the event
        GameManager.Instance.ActionManager.PlayerMove += UpdateActionValue;
        GameManager.Instance.ActionManager.MilestoneEvent += UpdateMileStoneScore;
    }
    
    ~ScoreManager()
    {
        // Unsubscribe to the event
        if (GameManager.Instance == null) return;
        GameManager.Instance.ActionManager.PlayerMove -= UpdateActionValue;
        GameManager.Instance.ActionManager.MilestoneEvent -= UpdateMileStoneScore;
    }
    
}
