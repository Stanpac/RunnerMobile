using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// This script is responsible for managing the game.
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    [BoxGroup("Camera")] 
    public CinemachineVirtualCamera _virtualCamera;
    
    [SerializeField, BoxGroup("StartParameters")]
    private float _startImpulsionForce = 10;
    
    [SerializeField, BoxGroup("Player")]
    private CarController _player;
    
    // Manager for the game
    public ActionManager actionManager {get; private set;}
    public UIManager uiManager {get; private set;}
    public InputManager inputManager {get; private set;}
    public GameStateManager gameStateManager {get; private set;}
    public MySceneManager mySceneManager {get; private set;}
    public PlayerManager playerManager {get; private set;}
    public TimerManager timerManager {get; private set;}
    public TileManager tileManager {get; set;}
    
    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(gameObject);
        }
        
        // Init Managers
        actionManager = new ActionManager();
        inputManager = new InputManager();
        gameStateManager = new GameStateManager();
        mySceneManager = new MySceneManager();
        playerManager = new PlayerManager();
        
        // TODO : remove monoBehaviour from the managers if possible 
        timerManager = gameObject.AddComponent<TimerManager>();
        
        
        uiManager = FindObjectOfType<UIManager>();
        
        LoadData();
    }

    private void LoadData()
    {
        playerManager._carPrefab = _player;
        gameStateManager.SetGameState(EGameState.GS_StartMenu);
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
