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
    
    [SerializeField][BoxGroup("Start Parameters")]
    private float _startImpulsionForce = 10;
    
    [SerializeField][BoxGroup("Player")]
    private CarController _player;
    
    [SerializeField][BoxGroup("All Luggages")]
    private LuggageCategories _luggageCategories;
    
    [SerializeField][BoxGroup("Scene")]
    [Tooltip("The scenes of the game")]
    private FSceneData[] _scenes;
    
    [SerializeField][ReadOnly]
    private bool _newGame = true;
    
    public bool NewGame => _newGame;
    public LuggageCategories LuggageCategories => _luggageCategories;
    public FSceneData[] Scenes => _scenes;
    
    // Manager for the game
    public ActionManager ActionManager {get; private set;}
    public UIManager UIManager {get; private set;}
    public InputManager InputManager {get; private set;}
    public GameStateManager GameStateManager {get; private set;}
    public MySceneManager MySceneManager {get; private set;}
    public PlayerManager PlayerManager {get; private set;}
    public TimerManager TimerManager {get; private set;}
    public ScoreManager ScoreManager {get; private set;}
    
    // Tile Generator 
    private TileCardGenerator _tileManager;
    public TileCardGenerator TileManager {
        
        get => _tileManager;
        set
        {
            if (_tileManager != null) {
                Debug.LogErrorFormat("there is already a tileManager in the GameManager");
                return;
            }
            _tileManager = value;
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
        ActionManager = new ActionManager();
        InputManager = new InputManager();
        GameStateManager = new GameStateManager();
        MySceneManager = new MySceneManager();
        PlayerManager = new PlayerManager();
        
        // TODO : load when the Game Start 
        ScoreManager = new ScoreManager();
        TimerManager = gameObject.AddComponent<TimerManager>();
        
        UIManager = FindObjectOfType<UIManager>();
        UIManager.enabled = true;
        
        LoadData();
    }

    private void LoadData()
    {
        PlayerManager.CarPrefab = _player;
        GameStateManager.SetGameState(EGameState.GS_StartMenu);
    }
    
    public void StartGame()
    {
        // TODO : Screen loading
        // Idee : Menu Demarage du jeu  avec la voiture qu'on va jouer,
        // Ecran de demarrage au debut histoire de tous charger avant 
        // Au start fade du menu, mouvement de Camera et hop ça start le jeu
        if (MySceneManager.LoadGameScene()) { 
            GameStateManager.SetGameState(EGameState.GS_Game);
            PlayerManager.InstantiatePlayer(Vector3.up * 2, Quaternion.identity);
            PlayerManager.GiveStartImpulsionToPlayer(Vector3.forward, _startImpulsionForce);
        } else {
            Debug.LogError("Game Scene not found");
        }
    }
    
    public void PauseGame()
    {
        
    }
    
    public void ReturnToMainMenu()
    {
        MySceneManager.UnloadGameScene();
        GameStateManager.SetGameState(EGameState.GS_StartMenu);
    }
    
    
    
}
