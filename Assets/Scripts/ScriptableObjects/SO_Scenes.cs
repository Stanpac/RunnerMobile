using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    public enum ESceneType
    {
        EST_StartScene,
        EST_Game,
    }

    [Serializable]
    public struct FSceneData
    {
        public ESceneType SceneType;
        [Scene] public string scene;
    }
    
    [CreateAssetMenu(fileName = "SO_Scenes", menuName = "ScriptableObjects/Scenes", order = 0)]
    public class SO_Scenes : ScriptableObject
    {
        [SerializeField, BoxGroup("Scene")] 
        [Tooltip("The scenes of the game")]
        public FSceneData[] _scenes;
    }
}