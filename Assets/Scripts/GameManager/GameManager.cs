using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// This script is responsible for managing the game.
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    
    [FormerlySerializedAs("camera")] [BoxGroup("Camera")] 
    public GameCamera _camera;
    
    [FormerlySerializedAs("StartImpulsionForce")] [SerializeField, BoxGroup("StartParameters")]
    private float _startImpulsionForce = 10;
    
    //Manager for the game
    public ActionManager actionManager {get; private set;}
    public UIManager uiManager {get; private set;}
    public InputManager inputManager {get; private set;}
    public SaveDataManager saveDataManager {get; private set;}
    public GameStateManager gameStateManager {get; private set;}
    public MySceneManager mySceneManager {get; private set;}
    public PlayerManager playerManager {get; private set;}
    public TileManager tileManager {get; set;}
    
    private void Awake()
    {
        // Singleton Pattern instance
        if (_instance == null) {
            _instance = this;
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
        playerManager._currentPlayerPrefab = saveDataManager._currentSoSave.player;
    }
    
    public void StartGame()
    {
        // TODO : Screen loading
        // Idee : Menu Demarage du jeu  avec la voiture qu'on va jouer,
        // Ecran de demarrage au debut histoire de tous charger avant 
        // Au start fade du menu, mouvement de Camera et hop ça start le jeu
        if (mySceneManager.loadGameScene()) { 
            gameStateManager.SetGameState(EGameState.GS_Game);
            playerManager.InstantiatePlayer(Vector3.up * 2, Quaternion.identity);
            playerManager.GiveStartImpulsionToPlayer(Vector3.forward, _startImpulsionForce);
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
        gameStateManager.SetGameState(EGameState.GS_StartMenu);
    }
}
