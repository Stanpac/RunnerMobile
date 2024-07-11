// using System;
// using UnityEngine;
// using UnityEngine.Serialization;
//
//
// [Serializable]
// public struct SpawnTileData
// {
//     public float _weight;
//     public float _price;
// }
//
// namespace ScriptableObjects
// {
//     [CreateAssetMenu(fileName = "Category", menuName = "Tile/TileCategory", order = 0)]
//     public class TileCategory : ScriptableObject
//     {
//         [SerializeField] private SpawnTileData[] _spawnTileDatas;
//         [SerializeField] private float _milestonesCondition;
//         
//         public float GetMilestonesCondition() => _milestonesCondition;
//         
//         public float GetCategoryWeight()
//         {
//             if (_spawnTileDatas.Length == 0) {
//                 return 0;
//             }
//             
//             float weight = 0;
//             foreach (var spawnTileData in _spawnTileDatas) {
//                 weight += spawnTileData._weight;
//             }
//
//             return weight;
//         }
//         
//         // TODO - Calcul Weight
//         // - X =  WeightCategory / TotalWeightCategory
//         // - Y =  WeightTileBlock / TotalWeightTileBlockInCategory 
//         // - ChanceOfSpawn = X * Y
//         public SpawnTileData GetTileBlock(float creaditsAvailable)
//         {
//             if (_spawnTileDatas.Length == 0) {
//                 return new SpawnTileData();
//             }
//             
//             float totalWeight = GetCategoryWeight();
//             float randomValue = UnityEngine.Random.Range(0, totalWeight);
//             float currentWeight = 0;
//             
//             foreach (var spawnTileData in _spawnTileDatas) {
//                 currentWeight += spawnTileData._weight;
//                 if (currentWeight >= randomValue) {
//                     return spawnTileData;
//                 }
//             }
//
//             return new SpawnTileData();
//         }
//     }
// }