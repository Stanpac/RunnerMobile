using System;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class MySceneManager 
{
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
    
    public bool LoadGameScene()
    {
        foreach (FSceneData sceneData in GameManager.Instance.Scenes) {
            if (sceneData._sceneType == ESceneType.Game) {
                LoadSceneAdditive(sceneData._scene);
                return true;
            }
        }
        return false;
    }
    
    public void UnloadGameScene()
    {
        foreach (FSceneData sceneData in GameManager.Instance.Scenes) {
            if (sceneData._sceneType == ESceneType.Game) {
                UnloadScene(sceneData._scene);
            }
        }
    }
}

public enum ESceneType
{
    Start,
    Game,
}

[Serializable]
public struct FSceneData
{
    [FormerlySerializedAs("SceneType")] 
    public ESceneType _sceneType;
    
    [FormerlySerializedAs("scene")] [Scene] 
    public string _scene;
}
    