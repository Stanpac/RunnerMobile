using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This script is responsible for managing the game.
 */
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField, BoxGroup("References")] private GameObject _startMenu;
    [SerializeField, BoxGroup("Scene In Start Menu"), Scene] private string[] _scenes;

    public GameObject _currentPlayer { get; set; }
    
    //Manager for the game
    public ActionManager actionManager;
    public UIManager uiManager;
    public InputManager inputManager;
    
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
        uiManager = gameObject.AddComponent<UIManager>();
    }

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneAdditive(string sceneName)
    {
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
    
}
