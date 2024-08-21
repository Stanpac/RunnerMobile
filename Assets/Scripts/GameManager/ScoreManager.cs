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
    private float _avRoadScoreMultiplier = 2f;
    
    // MileStone 
    private int _mileStoneIndex = 0;
    private float _avNextMilestoneCoef = 1.1f;
    
    // Coefficient of difficulty
    private float _baseCoefficientDiffiCulty = 2f;
    
    public float GetGlobalScore() => _globalscore;
    public int GetMileStoneIndex() => _mileStoneIndex;
    public float GetCoefDifficulty() => Mathf.Pow(_baseCoefficientDiffiCulty, _mileStoneIndex)  ;
    public void ResetActionValue() => _actionValue = 0;
    public void ResetMileStoneIndex() => _mileStoneIndex = 0;
    public void UpdateAvForNextMileStone() =>_avForNextMileStone += _avForNextMileStone * _avNextMilestoneCoef;
    public void UpdateMileStoneIndex() => _mileStoneIndex++;
    
    
    // TODO : Event call each frame by the player for update AV 
    // TODO : Event call a each milestone for update MilestoneScore
    
    public void UpdateScore()
    {
        // Score = AV + milestoneScore
        _globalscore = _actionValue + _milestoneScore;
        GameManager.Instance.ActionManager.InvokeScoreUpdate(_globalscore);
    }
    
    public void UpdateActionValue()
    {
        // AV += PosPlayer - PosPlayerLastFrame * CoefRoad
        UpdateScore();
    }
    
    public void UpdateMileStoneScore()
    {
        // MilestoneScore =  Y * Nbr de Baggage Actuel * MileStoneIndex
        UpdateScore();
    }
    
}
