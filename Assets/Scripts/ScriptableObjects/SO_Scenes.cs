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
        [FormerlySerializedAs("sceneType")] public ESceneType eSceneType;
        [Scene] public string scene;
    }
    
    [CreateAssetMenu(fileName = "SO_Scenes", menuName = "ScriptableObject/Scenes", order = 0)]
    public class SO_Scenes : ScriptableObject
    {
        [SerializeField, BoxGroup("Scene")] 
        public FSceneData[] _scenes;
    }
}