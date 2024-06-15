using System;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class MySceneManager : MonoBehaviour
{
    public SO_Scenes _scenesData; 
    
    private void Awake()
    {
        _scenesData = Resources.Load<SO_Scenes>("SO_Scenes");
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneAdditive(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded) return;
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
    
    public bool loadGameScene()
    {
        foreach (FSceneData sceneData in _scenesData.scenes) {
            if (sceneData.SceneType == ESceneType.EST_Game) {
                LoadSceneAdditive(sceneData.scene);
                return true;
            }
        }
        return false;
    }
    
    public void UnloadGameScene()
    {
        foreach (FSceneData sceneData in _scenesData.scenes) {
            if (sceneData.SceneType == ESceneType.EST_Game) {
                UnloadScene(sceneData.scene);
            }
        }
    }
}
    