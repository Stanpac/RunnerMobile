using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Lean.Touch;
using RoadTrip;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;


public class TileManager : MonoBehaviour
{
    private SO_TileManager _data;
    
    private Tile _previousTile;
    private List<Tile> _allSpawnedTiles;
    private PlayerController _currentplayer;
    
    
    private string _dataPath => "ScriptableObject/SO_TileManager";
    private string _startTilePath => "Tiles/Start";
    private string _lowLevelTilePath => "Tiles/Low";
    private string _mediumLevelTilePath => "Tiles/Medium";
    private string _hardLevelTilePath => "Tiles/Hard";

    private void Reset()
    {
        if (_data == null)
            _data = Resources.Load<SO_TileManager>(_dataPath);
    }

    private void Awake()
    {
        if (_data == null)
            _data = Resources.Load<SO_TileManager>(_dataPath);
        
        if (!_data.tileManualManagement)
            LoadallTiles();
        
        GameManager._instance.tileManager = this;
        _allSpawnedTiles = new List<Tile>();
    }
    
    private void Start()
    {
        _currentplayer = GameManager._instance.playerManager._currentPlayerController;
        GenerateStartTile();
        
        while (_previousTile.transform.position.z < _data.distanceoOfGeneration) {
            GenerateTile(UnityEngine.Random.Range(0, _data.allTiles.Length));
        } 
    }

    private void Update()
    {
        if (_previousTile.transform.position.z <_currentplayer.transform.position.z + _data.distanceoOfGeneration) {
            GenerateTile(UnityEngine.Random.Range(0, _data.allTiles.Length));
        }
    }
    
    private void GenerateStartTile() 
    {
        if (_data.startTile.Length <= 0) {
            Debug.LogError("No start tile found in the tile manager", this);
            return;
        }

        Vector3 pos = Vector3.zero;
       
        for (int i = 0; i < _data.startTile.Length; i++) {
            if (i != 0) pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z; // Don't Need for the First tile 
            pos.z += _data.startTile[i].GetComponentInChildren<Renderer>().bounds.extents.z;
            _previousTile = Instantiate<Tile>(_data.startTile[i], pos, Quaternion.identity, transform);
            _allSpawnedTiles.Add(_previousTile);
        }
    }
    
    private void GenerateTile( int index)
    {
        Vector3 pos = _previousTile.transform.position;
        pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z;
        pos.z += _data.allTiles[index].GetComponentInChildren<Renderer>().bounds.extents.z;
        _previousTile = Instantiate<Tile>(_data.allTiles[index], pos, Quaternion.identity, transform);
        _allSpawnedTiles.Add(_previousTile);
    }
    
    
    private void LoadallTiles()
    {
        _data.startTile = Resources.LoadAll<Tile>(_startTilePath);
        if (_data.startTile.Length <= 0)  {
            Debug.LogError("No start tile found in" + _startTilePath, this);
        }
        
        _data.lowLevelTiles = Resources.LoadAll<Tile>(_lowLevelTilePath);
        if (_data.lowLevelTiles.Length <= 0)  {
            Debug.LogError("No start tile found in" + _lowLevelTilePath, this);
        }
        
        _data.mediumLevelTiles = Resources.LoadAll<Tile>(_mediumLevelTilePath);
        if (_data.mediumLevelTiles.Length <= 0)  {
            Debug.LogError("No start tile found in" + _mediumLevelTilePath, this);
        }
        
        _data.hardLevelTiles = Resources.LoadAll<Tile>(_hardLevelTilePath);
        if (_data.hardLevelTiles.Length <= 0)  {
            Debug.LogError("No start tile found in" + _hardLevelTilePath, this);
        }
        
        _data.allTiles = _data.lowLevelTiles.Concat(_data.mediumLevelTiles).Concat(_data.hardLevelTiles).ToArray();
    }
}