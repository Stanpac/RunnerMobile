using System;
using CW.Common;
using UnityEngine;
using Lean.Touch;
using NaughtyAttributes;


public class ActionManager 
{
    // Finger
    public Action<LeanFinger> LastFingerUp;
    public Action<LeanFinger> FirstFingerDown;
    public Action<LeanFinger> FingerDown;
    
    // Game State
    public Action<EGameState, EGameState> GameStateUpdate;
    
    // Stability
    public Action <float> StabilityUpdate;
    public Action <bool> UnstableUpdate;
    
    // Luggage
    public Action<float> LuggageUpdate;
    
    // Player
    public Action PlayerDeath;
    
    // Score
    public Action<float> ScoreUpdate;
    
    public void InvokePlayerDeath()
    {
        PlayerDeath?.Invoke();
    }
    
    public void InvokeLastFingerUp(LeanFinger finger)
    {
        LastFingerUp?.Invoke(finger);
    }
    
    public void InvokeFirstFingerDown(LeanFinger finger)
    {
        FirstFingerDown?.Invoke(finger);
    }
    
    public void InvokeFingerDown(LeanFinger finger)
    {
        FingerDown?.Invoke(finger);
    }
    
    public void InvokeGameStateUpdate(EGameState previousGameState ,EGameState newGameState)
    {
        GameStateUpdate?.Invoke(previousGameState, newGameState);
    }
    
    public void InvokeStabilityUpdate(float stability)
    {
        StabilityUpdate?.Invoke(stability);
    }
    
    public void InvokeLuggageUpdate(float luggage)
    {
        LuggageUpdate?.Invoke(luggage);
    }
    
    public void InvokeUnstableUpdate(bool unstable)
    {
        UnstableUpdate?.Invoke(unstable);
    }
    
    public void InvokeScoreUpdate(float score)
    {
        ScoreUpdate?.Invoke(score);
    }
}
