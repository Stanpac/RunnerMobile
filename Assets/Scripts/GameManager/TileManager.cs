// using System;
// using System.Collections.Generic;
// using System.Linq;
// using NaughtyAttributes;
// using Lean.Touch;
// using ScriptableObjects;
// using UnityEngine;
// using UnityEngine.Serialization;
//
//
// public class TileManager : MonoBehaviour
// {
//     // TODO Data
//     // Distance Of Generation
//     
//     private GameObject _previousTileSpawned;
//     private List<GameObject> _allSpawnedTilesSpawned;
//     
//     private CarController Currentplayer
//     {
//         get
//         { 
//             CarController player = GameManager.Instance.playerManager._currentCarController;
//             if (player != null) {
//                 return player;
//             }
//             
//             // TODO : Change return type can't be null
//             return null;
//         }
//     }
//     
//     private void Awake()
//     {
//         //GameManager.Instance.TileManager = this;
//         _allSpawnedTilesSpawned = new List<GameObject>();
//     }
//
//     private void  StartGeneration()
//     {
//         
//     }
//     
//     private void Start()
//     {
//         GenerateStartTile();
//         
//         while (_previousTileSpawned.transform.position.z < _data.distanceoOfGeneration) {
//             GenerateTile(UnityEngine.Random.Range(0, _data.allTiles.Length));
//         }
//     }
//
//     private void Update()
//     {
//         if (_previousTileSpawned.transform.position.z <Currentplayer.transform.position.z + _data.distanceoOfGeneration) {
//             GenerateTile(UnityEngine.Random.Range(0, _data.allTiles.Length));
//         }
//         
//         CheckForTileDestruction();
//     }
//
//     private void CheckForTileDestruction()
//     {
//         for (int i = 0; i < _allSpawnedTilesSpawned.Count; i++) {
//             if (_allSpawnedTilesSpawned[i].transform.position.z < Currentplayer.transform.position.z - _data.distanceoOfGeneration) {
//                 Destroy(_allSpawnedTilesSpawned[i].gameObject);
//                 _allSpawnedTilesSpawned.Remove(_allSpawnedTilesSpawned[i]);
//             }
//         }
//     }
//
//     private void GenerateStartTile() 
//     {
//         if (_data.startTile.Length <= 0) {
//             Debug.LogError("No start tile found in the tile manager", this);
//             return;
//         }
//
//         Vector3 pos = Vector3.zero;
//        
//         for (int i = 0; i < _data.startTile.Length; i++) {
//             if (i != 0) pos.z += _previousTileSpawned.GetComponentInChildren<Renderer>().bounds.extents.z; // Don't Need for the First tile 
//             pos.z += _data.startTile[i].GetComponentInChildren<Renderer>().bounds.extents.z;
//             _previousTileSpawned = Instantiate<RoadTile>(_data.startTile[i], pos, Quaternion.identity, transform);
//             _allSpawnedTilesSpawned.Add(_previousTileSpawned);
//         }
//     }
//     
//     private void GenerateTile( int index)
//     {
//         Vector3 pos = _previousTileSpawned.transform.position;
//         pos.z += _previousTileSpawned.GetComponentInChildren<Renderer>().bounds.extents.z;
//         pos.z += _data.allTiles[index].GetComponentInChildren<Renderer>().bounds.extents.z;
//         _previousTileSpawned = Instantiate<RoadTile>(_data.allTiles[index], pos, Quaternion.identity, transform);
//         _allSpawnedTilesSpawned.Add(_previousTileSpawned);
//     }
//     
//     
//     private void LoadallTiles()
//     {
//         _data.startTile = Resources.LoadAll<RoadTile>(_startTilePath);
//         if (_data.startTile.Length <= 0)  {
//             Debug.LogError("No tile found in" + _startTilePath, this);
//         }
//         
//         _data.lowLevelTiles = Resources.LoadAll<RoadTile>(_lowLevelTilePath);
//         if (_data.lowLevelTiles.Length <= 0)  {
//             Debug.LogWarning("No tile found in" + _lowLevelTilePath, this);
//         }
//         
//         _data.mediumLevelTiles = Resources.LoadAll<RoadTile>(_mediumLevelTilePath);
//         if (_data.mediumLevelTiles.Length <= 0)  {
//             Debug.LogWarning("No tile found in" + _mediumLevelTilePath, this);
//         }
//         
//         _data.hardLevelTiles = Resources.LoadAll<RoadTile>(_hardLevelTilePath);
//         if (_data.hardLevelTiles.Length <= 0)  {
//             Debug.LogWarning("No tile found in" + _hardLevelTilePath, this);
//         }
//         
//         _data.allTiles = _data.lowLevelTiles.Concat(_data.mediumLevelTiles).Concat(_data.hardLevelTiles).ToArray();
//     }
// }