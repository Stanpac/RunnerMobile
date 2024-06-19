using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using RoadTrip;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_TileManager", menuName = "ScriptableObjects/TileManager", order = 0)]
    public class SO_TileManager : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters")]
        [Tooltip("Distance of generation of the tiles in the game")]
        public float distanceoOfGeneration = 100;
        
        [FormerlySerializedAs("TileManualManagement")]
        [SerializeField, BoxGroup("Parameters")]
        [Tooltip("Distance of generation of the tiles in the game")]
        public bool tileManualManagement = true;
        
        [FormerlySerializedAs("AllTiles")]
        [SerializeField, BoxGroup("Tile"), EnableIf("tileManualManagement")]
        [Tooltip("All the tiles that will be generated in the game")]
        public Tile[] allTiles;
        
        [SerializeField, BoxGroup("Tile"), EnableIf("tileManualManagement")]
        [Tooltip("The first tile that will be generated in the game")]
        public Tile[] startTile;
        
        [FormerlySerializedAs("LowLevelTiles")]
        [SerializeField, BoxGroup("Tile"), EnableIf("tileManualManagement")]
        [Tooltip("Tile of Low Level")]
        public Tile[] lowLevelTiles;
        
        [FormerlySerializedAs("MediumLevelTiles")]
        [SerializeField, BoxGroup("Tile"), EnableIf("tileManualManagement")]
        [Tooltip("Tile of Medium Level")]
        public Tile[] mediumLevelTiles;
        
        [FormerlySerializedAs("HardLevelTiles")]
        [SerializeField, BoxGroup("Tile"), EnableIf("tileManualManagement")]
        [Tooltip("Tile of Hard Level")]
        public Tile[] hardLevelTiles;
    }
}

