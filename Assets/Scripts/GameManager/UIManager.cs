using System;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;


public class UIManager : MonoBehaviour 
{
     [SerializeField, BoxGroup("StartMenu")] private GameObject _startMenu;
     [SerializeField, BoxGroup("StartMenu")] private GameObject _startMenuFond;
     [SerializeField, BoxGroup("StartMenu")] private GameObject _startMenuButton;

     [SerializeField, BoxGroup("Gameplay")] private GameObject _gameUI;
     [SerializeField, BoxGroup("Gameplay")] private GameObject _menuButton;
     //[SerializeField, BoxGroup("Gameplay")] private GameObject _WheelController;

     private void OnEnable()
     {
         GameManager._instance.actionManager.OnGameStateChange += GameStateChange;
     }

     private void OnDisable()
     {
         GameManager._instance.actionManager.OnGameStateChange -= GameStateChange;
     }
     
     private void Awake()
     {
         _startMenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager._instance.StartGame());
         _menuButton.GetComponent<Button>().onClick.AddListener(() => GameManager._instance.ReturnToMainMenu());
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
}
