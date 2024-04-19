using System;
using CW.Common;
using UnityEngine;
using Lean.Touch;
using NaughtyAttributes;



/*
 * This script is responsible for managing the actions in the game.
 */
public class ActionManager : MonoBehaviour
{
    public Action OnPlayerDeath;
    public Action<LeanFinger> OnLastFingerUp;
    public Action<LeanFinger> OnFirstFingerDown;
    public Action<LeanFinger> OnFingerDown;
    public Action<GameState, GameState> OnGameStateChange;
    
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
    
    public void GameStateChange(GameState PreviousGameState ,GameState NewGameState)
    {
        OnGameStateChange?.Invoke(PreviousGameState, NewGameState);
    }
    
}
