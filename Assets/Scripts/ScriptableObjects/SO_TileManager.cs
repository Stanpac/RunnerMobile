using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TileGeneratorValue", menuName = "ScriptableObject/TileGeneratorValue", order = 0)]
    public class SO_TileManager : ScriptableObject
    {
        [SerializeField, BoxGroup("Tile")]
        [Tooltip("All the tiles that will be generated in the game")]
        public Tile[] _tiles;
        
        [SerializeField, BoxGroup("Tile")]
        [Tooltip("Distance of generation of the tiles in the game")]
        public float _distanceoOfGeneration = 100;
    }
}

