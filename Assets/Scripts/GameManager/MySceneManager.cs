using System;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MySceneManager : MonoBehaviour
{

    public SO_Scenes _ScenesData; //{get; private set;}
    
    private void Awake()
    {
        _ScenesData = Resources.Load<SO_Scenes>("SO_Scenes");
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
        foreach (FSceneData sceneData in _ScenesData._scenes) {
            if (sceneData.SceneType == ESceneType.EST_Game) {
                LoadSceneAdditive(sceneData.scene);
                return true;
            }
        }
        return false;
    }
    
    public void UnloadGameScene()
    {
        foreach (FSceneData sceneData in _ScenesData._scenes) {
            if (sceneData.SceneType == ESceneType.EST_Game) {
                UnloadScene(sceneData.scene);
            }
        }
    }
}
    