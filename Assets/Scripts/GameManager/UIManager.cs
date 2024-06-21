using System;
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
    
    // debug
    [SerializeField, BoxGroup("Debug")] 
    private TMP_Text _debugStability;
    
    [SerializeField, BoxGroup("Debug")]
    private TMP_Text _debugLuggage;
    
    [SerializeField, BoxGroup("Debug")]
    private GameObject _debugUnstable;

    //[SerializeField, BoxGroup("Gameplay")] private GameObject _WheelController;
    
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
        _startMenu.SetActive(active);
        _startMenuFond.SetActive(active); 
        _startMenuButton.SetActive(active);
    }
    
    private void SetGameUIActive(bool active)
    {
        _gameUI.SetActive(active);
        _menuButton.SetActive(active);
        //_WheelController.SetActive(active);
    }
    
    private void GameStateChange(EGameState PreviousGameState ,EGameState NewGameState)
    {
        switch (PreviousGameState) {
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
    
    private void StabilityChange(float stability)
    {
        _debugStability.text = "Stability: "  + stability.ToString();
    }
    
    private void LuggageChange(int luggage)
    {
        _debugLuggage.text = "Luggage: " + luggage.ToString();
    }
    private void OnUnstableChange(bool unstable)
    {
        _debugUnstable.SetActive(unstable);
    }
    
    private void OnEnable()
    {
        GameManager.Instance.actionManager.OnGameStateChange += GameStateChange;
        GameManager.Instance.actionManager.OnStabilityChange += StabilityChange;
        GameManager.Instance.actionManager.OnLuggageChange += LuggageChange;
        GameManager.Instance.actionManager.OnUnstableChange += OnUnstableChange;
    }


    private void OnDisable()
    {
        GameManager.Instance.actionManager.OnGameStateChange -= GameStateChange;
        GameManager.Instance.actionManager.OnStabilityChange -= StabilityChange;
        GameManager.Instance.actionManager.OnLuggageChange -= LuggageChange;
        GameManager.Instance.actionManager.OnUnstableChange -= OnUnstableChange;
    }
}
