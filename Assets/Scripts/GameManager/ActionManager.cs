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
    public Action<float, float> LuggageUpdate;
    
    // Player
    public Action PlayerDeath;
    public Action<float, float, bool> PlayerMove;
    
    // Score
    public Action<float> ScoreUpdate;
    
    // Milestone
    public Action<int> MilestoneEvent;
    public Action<float> AvForNextMilestoneReached;
    public Action<Texture2D[]> GainLuggage;
    
    // PowerUp
    public Action<float> PowerUpStartEvent;
    
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
    
    public void InvokePlayerDeath()
    {
        PlayerDeath?.Invoke();
    }
    
    public void InvokePlayerMove(float playerPosZ, float playerPosLastFrameZ, bool isOnRoad)
    {
        PlayerMove?.Invoke(playerPosZ, playerPosLastFrameZ, isOnRoad);
    }
    
    public void InvokeGameStateUpdate(EGameState previousGameState ,EGameState newGameState)
    {
        GameStateUpdate?.Invoke(previousGameState, newGameState);
    }
    
    public void InvokeStabilityUpdate(float stability)
    {
        StabilityUpdate?.Invoke(stability);
    }
    
    public void InvokeLuggageUpdate(float luggage, float weight)
    {
        LuggageUpdate?.Invoke(luggage, weight);
    }
    
    public void InvokeUnstableUpdate(bool unstable)
    {
        UnstableUpdate?.Invoke(unstable);
    }
    
    public void InvokeScoreUpdate(float score)
    {
        ScoreUpdate?.Invoke(score);
    }
    
    public void InvokeMilestoneEvent(int luggageCount)
    {
        MilestoneEvent?.Invoke(luggageCount);
    }
    
    public void InvokeGainLuggage(Texture2D[] luggageTextures)
    {
        GainLuggage?.Invoke(luggageTextures);
    }
    
    public void InvokeAvForNextMilestoneReached(float avForNextMilestone)
    {
        AvForNextMilestoneReached?.Invoke(avForNextMilestone);
    }
    
    public void InvokePowerUpStartEvent(float duration)
    {
        PowerUpStartEvent?.Invoke(duration);
    }
}
