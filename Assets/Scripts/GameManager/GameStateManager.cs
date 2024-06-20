using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public EGameState CurrentGameState { get; private set; }
    
    private void Awake()
    {
        SetGameState(EGameState.GS_StartMenu);
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
    GS_StartMenu,
    GS_Game,
    GS_Pause,
    GS_GameOver,
}
