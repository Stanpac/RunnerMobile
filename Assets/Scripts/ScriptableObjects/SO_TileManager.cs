using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_TileManager", menuName = "ScriptableObjects/TileManager", order = 0)]
    public class SO_TileManager : ScriptableObject
    {
        [SerializeField, BoxGroup("Tile")]
        [Tooltip("All the tiles that will be generated in the game")]
        public Tile[] tiles;
        
        [SerializeField, BoxGroup("Start")]
        [Tooltip("The first tile that will be generated in the game")]
        public Tile[] startTile;
        
        [SerializeField, BoxGroup("Tile")]
        [Tooltip("Distance of generation of the tiles in the game")]
        public float distanceoOfGeneration = 100;
    }
}

