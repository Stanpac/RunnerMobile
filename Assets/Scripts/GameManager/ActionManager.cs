using System;
using CW.Common;
using UnityEngine;
using Lean.Touch;
using NaughtyAttributes;


public class ActionManager : MonoBehaviour
{
    public Action OnPlayerDeath;
    public Action<LeanFinger> OnLastFingerUp;
    public Action<LeanFinger> OnFirstFingerDown;
    public Action<LeanFinger> OnFingerDown;
    public Action<EGameState, EGameState> OnGameStateChange;
    public Action <float> OnStabilityChange;
    
    public void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }
    
    public void LastFingerUp(LeanFinger finger)
    {
        OnLastFingerUp?.Invoke(finger);
    }
    
    public void FirstFingerDown(LeanFinger finger)
    {
        OnFirstFingerDown?.Invoke(finger);
    }
    
    public void FingerDown(LeanFinger finger)
    {
        OnFingerDown?.Invoke(finger);
    }
    
    public void GameStateChange(EGameState PreviousGameState ,EGameState NewGameState)
    {
        OnGameStateChange?.Invoke(PreviousGameState, NewGameState);
    }
    
    public void StabilityChange(float stability)
    {
        OnStabilityChange?.Invoke(stability);
    }
    
}
