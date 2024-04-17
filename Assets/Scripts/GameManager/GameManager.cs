using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/*
 * This script is responsible for managing the game.
 */
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField, BoxGroup("References")] private GameObject _startMenu;
    [SerializeField, BoxGroup("Scene In Start Menu"), Scene] private string[] _scenes;
    
    private GameObject _player;
    private LeanTouch _leanTouch;
    
    private void Awake()
    {
        // Singleton Pattern instance
        if (instance == null) {
            instance = this;
        } else  {
            Destroy(gameObject);
        }
        
        // Find Lean Touch in the scene or add it if it doesn't exist
        _leanTouch = FindObjectOfType<LeanTouch>();
        if (_leanTouch == null) {
            _leanTouch = gameObject.AddComponent<LeanTouch>();
        }
    }

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void Loadasync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    
    public void UnloadSceneAsync(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
