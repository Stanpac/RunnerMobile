using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Lean.Touch;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;


/*
 * This script is responsible for generating the tiles
 * And manage there movement
 */
public class TileManager : MonoBehaviour
{
    private SO_TileManager _data;
    
    private GameObject _previousTile;
    private List<GameObject> _allTiles;
    private GameObject _currentplayer;

    private void Reset()
    {
        if (_data == null)
            _data = Resources.Load<SO_TileManager>("SO_TileManager");
    }

    private void Awake()
    {
        if (_data == null)
            _data = Resources.Load<SO_TileManager>("SO_TileManager");
        
        GameManager._instance.tileManager = this;
        _allTiles = new List<GameObject>();
    }
    
    private void Start()
    {
        _currentplayer = GameManager._instance.playerManager._currentPlayerController.gameObject;
        GenerateStartTile();
        
        while (_previousTile.transform.position.z < _data.distanceoOfGeneration) {
            GenerateTile(UnityEngine.Random.Range(0, _data.tiles.Length));
        } 
    }

    private void Update()
    {
        if (_previousTile.transform.position.z <_currentplayer.transform.position.z + _data.distanceoOfGeneration) {
            GenerateTile(UnityEngine.Random.Range(0, _data.tiles.Length));
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
            pos.z += _data.startTile[i].TilePrefab.GetComponentInChildren<Renderer>().bounds.extents.z;
            _previousTile = Instantiate<GameObject>(_data.startTile[i].TilePrefab, pos, Quaternion.identity, transform);
            _allTiles.Add(_previousTile);
        }
    }
    
    private void GenerateTile( int index)
    {
        Vector3 pos = _previousTile.transform.position;
        pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z;
        pos.z += _data.tiles[index].TilePrefab.GetComponentInChildren<Renderer>().bounds.extents.z;
        _previousTile = Instantiate<GameObject>(_data.tiles[index].TilePrefab, pos, Quaternion.identity, transform);
        _allTiles.Add(_previousTile);
    }
    
}

[Serializable]
public struct Tile
{
    public GameObject TilePrefab;
}