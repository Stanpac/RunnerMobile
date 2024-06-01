using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is responsible for managing the game.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [BoxGroup("Camera")] 
    public GameCamera camera;
    
    [SerializeField, BoxGroup("StartParameters")]
    private float StartImpulsionForce = 10;
    
    //Manager for the game
    public ActionManager actionManager {get; private set;}
    public UIManager uiManager {get; private set;}
    public InputManager inputManager {get; private set;}
    public SaveDataManager saveDataManager {get; private set;}
    public GameStateManager gameStateManager {get; private set;}
    public MySceneManager mySceneManager {get; private set;}
    public PlayerManager playerManager {get; private set;}
    public TileManager TileManager {get; set;}
    
    
    private void Awake()
    {
        // Singleton Pattern instance
        if (instance == null) {
            instance = this;
        } else  {
            Destroy(gameObject);
        }
        
        // Instantiate All the Managers for the game
        saveDataManager = GetComponent<SaveDataManager>();
        actionManager = gameObject.AddComponent<ActionManager>();
        inputManager = gameObject.AddComponent<InputManager>();
        gameStateManager = gameObject.AddComponent<GameStateManager>();
        
        mySceneManager = gameObject.AddComponent<MySceneManager>();
        
        playerManager = gameObject.AddComponent<PlayerManager>();
        uiManager = FindObjectOfType<UIManager>();
        
        // load the save data
        playerManager._currentPlayerPrefab = saveDataManager.currentSoSave.Player;
    }
    
    public void StartGame()
    {
        // TODO : Screen loading
        // Idee : Menu Demarage du jeu  avec la voiture qu'on va jouer,
        // Ecran de demarrage au debut histoire de tous charger avant 
        // Au start fade du menu, mouvement de Camera et hop ça start le jeu
        if (mySceneManager.loadGameScene()) { 
            gameStateManager.SetGameState(GameState.Game);
            playerManager.InstantiatePlayer(Vector3.up * 2, Quaternion.identity);
            playerManager.GiveStartImpulsionToPlayer(Vector3.forward, StartImpulsionForce);
        } else {
            Debug.LogError("Game Scene not found");
        }
    }
    
    public void PauseGame()
    {
        
    }
    
    public void ReturnToMainMenu()
    {
        mySceneManager.UnloadGameScene();
        gameStateManager.SetGameState(GameState.StartMenu);
    }
}
