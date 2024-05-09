using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObject/Save", order = 0)]
    public class SO_Save : ScriptableObject
    {
        public GameObject Player;
    }
}