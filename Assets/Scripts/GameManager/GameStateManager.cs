using UnityEngine;

/*------------------- Struct / enum -------------------*/
public enum GameState
{
    StartMenu,
    Game,
    Pause,
    GameOver,
}

/*------------------- End Struct / enum -------------------*/
public class GameStateManager : MonoBehaviour
{
    public GameState CurrentGameState { get; private set; }
    
    private void Awake()
    {
        SetGameState(GameState.StartMenu);
    }
    
    public void SetGameState(GameState gameState)
    {
        GameState previousGameState = CurrentGameState;
        CurrentGameState = gameState;
        GameManager.instance.actionManager.GameStateChange(previousGameState,gameState);
    }
}
