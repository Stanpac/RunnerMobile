using System;
using UnityEngine;
using NaughtyAttributes;

public class ScoreManager 
{
    // _score = _actionValue + _score + (_luggageAv * (luggageWeight));
    private float _score = 0;
    
    private float _actionValue = 0;
    private float _avForNextMileStone = 1000;
    
    private float _luggageAv = 0;
    
    
    // Coef Difficulty = Mathf.Pow(_coefficientDiffiCulty, _mileStoneIndex)
    private int _mileStoneIndex = 0;
    private float _coefficientDiffiCulty = 2.15f;
    
    private LuggageHandler _luggageHandler;
    
    // TODO : reset _Action value when the player is at a MileStone (After Update the Score)
    // TODO : Update _mileStoneIndex 
    // TODO : Update Finish Calculate the _avForNextMileStone ? 
    
    public float GetScore() => _score;
    public int GetMileStoneIndex() => _mileStoneIndex;
    
    public float GetCoefDifficulty() => Mathf.Pow(_coefficientDiffiCulty, _mileStoneIndex);
    
    public void UpdateScore()
    {
        if (_luggageHandler== null) {
            _luggageHandler = GameManager.Instance.playerManager._currentCarController.GetComponent<LuggageHandler>();
        }
        
        // TODO : Update the Calcul when the Weight is Add To the LuggageHandler (Now 1 luggage = 1 weight)
        _score = _score + _actionValue + (_luggageAv * _luggageHandler.Luggage);
        
        // TODO : Event For Update Score on UI
    }
    
    public void UpdateAvForNextMileStone()
    {
        _avForNextMileStone += _avForNextMileStone * _mileStoneIndex;
    }
    
}
