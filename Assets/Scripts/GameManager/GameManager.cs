using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

/*------------------- Struct / enum -------------------*/
public enum SceneType
{
    StartScene,
    Game,
}

[Serializable]
public struct SceneData
{
    public SceneType sceneType;
    [Scene] public string scene;
}
/*------------------- End Struct / enum -------------------*/

/*
 * This script is responsible for managing the game.
 */
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField, BoxGroup("Scene")] private SceneData[] _scenes;
    [BoxGroup("Camera")] public GameCamera camera;
    
    public GameObject _currentPlayerPrefab { get; set; }
    
    //Manager for the game
    public ActionManager actionManager {get; private set;}
    public UIManager uiManager {get; private set;}
    public InputManager inputManager {get; private set;}
    public SaveDataManager saveDataManager {get; private set;}
    public GameStateManager gameStateManager {get; private set;}
    
    public TileGenerator tileGenerator {get; set;}
    
    
    private void Awake()
    {
        // Singleton Pattern instance
        if (instance == null) {
            instance = this;
        } else  {
            Destroy(gameObject);
        }
        
        // Instantiate All the Managers for the game
        actionManager = gameObject.AddComponent<ActionManager>();
        inputManager = gameObject.AddComponent<InputManager>();
        saveDataManager = GetComponent<SaveDataManager>();
        gameStateManager = gameObject.AddComponent<GameStateManager>();

        uiManager = FindObjectOfType<UIManager>();
        
        // Attach Action 
        actionManager.OnGameStateChange += GameStateChange;
        
        // load the save data
        saveDataManager.Load();
        _currentPlayerPrefab = saveDataManager._CurrentSaveData.Player;
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneAdditive(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded) return;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
    
    public void RealoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void GameStateChange(GameState PreviousGameState ,GameState NewGameState)
    {
        if (NewGameState == GameState.Game) {
            foreach (SceneData scene in _scenes) {
                if (scene.sceneType == SceneType.Game) {
                    LoadSceneAdditive(scene.scene);
                }
            }
        } else if (PreviousGameState == GameState.Game) {
            foreach (SceneData scene in _scenes) {
                if (scene.sceneType == SceneType.Game) {
                    UnloadScene(scene.scene);
                }
            }
        }
    }
    
}
