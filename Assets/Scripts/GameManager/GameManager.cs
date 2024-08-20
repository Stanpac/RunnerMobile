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
    public static GameManager Instance { get; private set; }
    
    [BoxGroup("Camera")] 
    public CinemachineVirtualCamera _virtualCamera;
    
    [SerializeField, BoxGroup("StartParameters")]
    private float _startImpulsionForce = 10;
    
    [SerializeField, BoxGroup("Player")]
    private CarController _player;
    
    [SerializeField, BoxGroup("AllLuggage")]
    private LuggageCategories _luggageCategories;
    
    [SerializeField, ReadOnly]
    private bool _newGame = true;
    public bool NewGame => _newGame;
    public LuggageCategories LuggageCategories => _luggageCategories;
    
    // Manager for the game
    public ActionManager actionManager {get; private set;}
    public UIManager uiManager {get; private set;}
    public InputManager inputManager {get; private set;}
    public GameStateManager gameStateManager {get; private set;}
    public MySceneManager mySceneManager {get; private set;}
    public PlayerManager playerManager {get; private set;}
    public TimerManager timerManager {get; private set;}
    public ScoreManager scoreManager {get; private set;}
    
    // Tile Generator 
    private TileCardGenerator tileManager;
    public TileCardGenerator TileManager {
        
        get => tileManager;
        set
        {
            if (tileManager != null) {
                Debug.LogErrorFormat("there is already a tileManager in the GameManager");
                return;
            }
            tileManager = value;
        }
    }
    
    private void OnEnable()
    {
        if (!Instance) {
            Instance = this;
        } else {
            Debug.LogErrorFormat(this, "Duplicate instance of singleton class {0}. Only one should exist at a time.", GetType().Name);
        }
    }

    private void OnDisable()
    {
        if (Instance != this)
            return;
        
        Instance = null;
    }
    
    private void Start()
    {
        // Init Managers
        actionManager = new ActionManager();
        inputManager = new InputManager();
        gameStateManager = new GameStateManager();
        mySceneManager = new MySceneManager();
        playerManager = new PlayerManager();
        scoreManager = new ScoreManager();
        
        timerManager = gameObject.AddComponent<TimerManager>();
        
        uiManager = FindObjectOfType<UIManager>();
        uiManager.enabled = true;
        
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
