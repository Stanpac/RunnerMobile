using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_Save", menuName = "ScriptableObject/Save", order = 0)]
    public class SO_Save : ScriptableObject
    {
        public GameObject Player;
    }
}