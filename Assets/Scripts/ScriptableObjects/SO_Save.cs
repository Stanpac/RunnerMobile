using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_Save", menuName = "ScriptableObjects/Save", order = 0)]
    public class SO_Save : ScriptableObject
    {
        public GameObject player;
    }
}