using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObject/SaveData", order = 0)]
    public class SaveData : ScriptableObject
    {
        public GameObject Player;
    }
}