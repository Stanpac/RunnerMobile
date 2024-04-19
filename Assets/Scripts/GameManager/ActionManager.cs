using System;
using UnityEngine;
using NaughtyAttributes;



/*
 * This script is responsible for managing the actions in the game.
 */
public class ActionManager : MonoBehaviour
{
    public Action OnPlayerDeath;
    public Action OnLastFingerUp;
    public Action OnFirstFingerDown;
    public Action<GameState, GameState> OnGameStateChange;
    
    public void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }
    
    public void LastFingerUp()
    {
        OnLastFingerUp?.Invoke();
    }
    
    public void FirstFingerDown()
    {
        OnFirstFingerDown?.Invoke();
    }
    
    public void GameStateChange(GameState PreviousGameState ,GameState NewGameState)
    {
        OnGameStateChange?.Invoke(PreviousGameState, NewGameState);
    }
    
}
