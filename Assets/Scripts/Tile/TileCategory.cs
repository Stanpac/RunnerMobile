using System;
using UnityEngine;


[Serializable]
public struct SpawnTileData
{
    public TileBlock _tileBlock;
    public float _weight;
    public float _price;
    public float _minScore;
}

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Category", menuName = "Tile/TileCategory", order = 0)]
    public class TileCategory : ScriptableObject
    {
        [SerializeField] private SpawnTileData[] _spawnTileDatas;
        
        
        public float GetCategoryWeight()
        {
            if (_spawnTileDatas.Length == 0) {
                return 0;
            }
            
            float weight = 0;
            foreach (var spawnTileData in _spawnTileDatas) {
                weight += spawnTileData._weight;
            }

            return weight;
        }
        
    }
}