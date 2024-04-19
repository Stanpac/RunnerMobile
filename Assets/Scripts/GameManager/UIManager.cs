using System;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;

/*
 * This script is responsible for managing the UI in the game.
 */
public class UIManager : MonoBehaviour 
{
     [SerializeField, BoxGroup("StartMenu")] private GameObject _startMenu;
     [SerializeField, BoxGroup("StartMenu")] private GameObject _startMenuFond;
     [SerializeField, BoxGroup("StartMenu")] private GameObject _startMenuButton;

     [SerializeField, BoxGroup("Gameplay")] private GameObject _GameUI;
     [SerializeField, BoxGroup("Gameplay")] private GameObject _MenuButton;
     //[SerializeField, BoxGroup("Gameplay")] private GameObject _WheelController;

     private void Awake()
     {
         _startMenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.gameStateManager.SetGameState(GameState.Game));
         _MenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.gameStateManager.SetGameState(GameState.StartMenu));
     }

     private void Start()
     {
         GameManager.instance.actionManager.OnGameStateChange += GameStateChange;
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
         _GameUI.SetActive(active);
         _MenuButton.SetActive(active);
         //_WheelController.SetActive(active);
     }
     
     private void GameStateChange(GameState PreviousGameState ,GameState NewGameState)
     {
            switch (PreviousGameState)
            {
                case GameState.StartMenu:
                    SetStartMenuActive(false);
                    break;
                case GameState.Game:
                    SetGameUIActive(false);
                    break;
                case GameState.Pause:
                    break;
                case GameState.GameOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (NewGameState)
            {
                case GameState.StartMenu:
                    SetStartMenuActive(true);
                    break;
                case GameState.Game:
                    SetGameUIActive(true);
                    break;  
                case GameState.Pause:
                    break;
                case GameState.GameOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
     }
}
