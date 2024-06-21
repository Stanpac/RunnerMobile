using System;
using CW.Common;
using UnityEngine;
using Lean.Touch;
using NaughtyAttributes;


public class ActionManager : MonoBehaviour
{
    
    // Finger
    public Action<LeanFinger> OnLastFingerUp;
    public Action<LeanFinger> OnFirstFingerDown;
    public Action<LeanFinger> OnFingerDown;
    
    // Game State
    public Action<EGameState, EGameState> OnGameStateChange;
    
    // Stability
    public Action <float> OnStabilityChange;
    public Action <bool> OnUnstableChange;
    
    // Luggage
    public Action<int> OnLuggageChange;
    
    // Player
    public Action OnPlayerDeath;
    
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
    
    public void LuggageChange(int luggage)
    {
        OnLuggageChange?.Invoke(luggage);
    }
    
    public void UnstableChange(bool unstable)
    {
        OnUnstableChange?.Invoke(unstable);
    }
}
