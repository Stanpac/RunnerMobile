using UnityEngine;

public class GameStateManager
{
    public EGameState CurrentGameState { get; private set; }
    
    public GameStateManager()
    {
        SetGameState(EGameState.GS_loadding);
    }
    
    public void SetGameState(EGameState gameState)
    {
        EGameState previousGameState = CurrentGameState;
        CurrentGameState = gameState;
        GameManager.Instance.actionManager.GameStateChange(previousGameState,gameState);
    }
}

public enum EGameState
{
    GS_loadding,
    GS_StartMenu,
    GS_Game,
    GS_Pause,
    GS_GameOver,
}
