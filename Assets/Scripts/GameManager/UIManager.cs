using System;
using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;


public class UIManager : MonoBehaviour 
{
    // Start Menu
    [SerializeField, BoxGroup("StartMenu")]
    private GameObject _startMenu;
    [SerializeField, BoxGroup("StartMenu")] 
    private GameObject _startMenuFond;
    [SerializeField, BoxGroup("StartMenu")] 
    private GameObject _startMenuButton;
    
    // Game UI
    [SerializeField, BoxGroup("Gameplay")] 
    private GameObject _gameUI;
    [SerializeField, BoxGroup("Gameplay")] 
    private GameObject _menuButton;
    
    // Bonus UI 
    [SerializeField, BoxGroup("RoadBonus")]
    private RawImage _roadBonus;
    [SerializeField, BoxGroup("PowerUpBonus")]
    private RawImage _powerUpBonus;
    
    // Stability UI
    [SerializeField, BoxGroup("Stability")]
    private RectTransform _aiguille;
    
    // Score UI
    [SerializeField, BoxGroup("Score")]
    private TextMeshProUGUI _score;
    
    // Luggage UI
    [SerializeField, BoxGroup("Luggage")]
    private TextMeshProUGUI _nbrLuggage;
    
    
    private void Awake()
    {
        _startMenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.StartGame());
        _menuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ReturnToMainMenu());
    }
    
    private void Start()
    {
        SetStartMenuActive(true);
        SetGameUIActive(false);
    }
    
    private void SetStartMenuActive(bool active)
    {
        _startMenu?.SetActive(active);
        _startMenuFond?.SetActive(active); 
        _startMenuButton?.SetActive(active);
    }
    
    private void SetGameUIActive(bool active)
    {
        _gameUI?.SetActive(active);
        _menuButton?.SetActive(active);
    }
    
    private void OnGameStateChange(EGameState PreviousGameState ,EGameState NewGameState)
    {
        switch (PreviousGameState) {
            case EGameState.GS_loadding:
                break;
            case EGameState.GS_StartMenu:
                SetStartMenuActive(false);
                break;
            case EGameState.GS_Game:
                SetGameUIActive(false);
                break;
            case EGameState.GS_Pause:
                break;
            case EGameState.GS_GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
 
        switch (NewGameState) {
            case EGameState.GS_StartMenu:
                SetStartMenuActive(true);
                break;
            case EGameState.GS_Game:
                SetGameUIActive(true);
                break;  
            case EGameState.GS_Pause:
                break;
            case EGameState.GS_GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnStabilityChange(float stability)
    {
        if (_aiguille == null) return;
        _aiguille.localRotation = Quaternion.Euler(0, 0, stability * -90);
    }
    
    private void OnLuggageChange(float luggage, float weight)
    {
        if (_nbrLuggage == null) return;
        _nbrLuggage.text = luggage.ToString();
    }
    
    private void OnScoreChange(float score)
    {
        if (_score == null) return;
        _score.text = ((int)score).ToString();
    }
    private void OnPlayerMove(float playerPosZ, float playerPosLastFrameZ, bool isOnRoad)
    {
        if (_roadBonus == null) return;
        if (_roadBonus.enabled != isOnRoad) {
            _roadBonus.enabled = isOnRoad;
        }
    }
    private void OnPowerUpStart(float duration)
    {
        if (_powerUpBonus == null) return;
        _powerUpBonus.enabled = true;
        StartCoroutine(PowerUpDuration(duration));
    }
    
    IEnumerator PowerUpDuration(float duration)
    {
        yield return new WaitForSeconds(duration * 0.8f);
        _powerUpBonus.enabled = !_powerUpBonus.enabled;
        for (int i = 0; i < 10; i++) {
            yield return new WaitForSeconds(duration * 0.02f);
            _powerUpBonus.enabled = !_powerUpBonus.enabled;
        }
        _powerUpBonus.enabled = false;
    }
    
    private void OnEnable()
    {
        GameManager.Instance.ActionManager.GameStateUpdate += OnGameStateChange;
        GameManager.Instance.ActionManager.StabilityUpdate += OnStabilityChange;
        GameManager.Instance.ActionManager.LuggageUpdate += OnLuggageChange;
        GameManager.Instance.ActionManager.ScoreUpdate += OnScoreChange;
        GameManager.Instance.ActionManager.PlayerMove += OnPlayerMove;
        GameManager.Instance.ActionManager.PowerUpStartEvent += OnPowerUpStart;
    }
    
    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.ActionManager.GameStateUpdate -= OnGameStateChange;
        GameManager.Instance.ActionManager.StabilityUpdate -= OnStabilityChange;
        GameManager.Instance.ActionManager.LuggageUpdate -= OnLuggageChange;
        GameManager.Instance.ActionManager.ScoreUpdate -= OnScoreChange;
        GameManager.Instance.ActionManager.PlayerMove -= OnPlayerMove;
        GameManager.Instance.ActionManager.PowerUpStartEvent -= OnPowerUpStart;
    }
}
