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
    private SO_TileManager Data;
    
    private GameObject _previousTile;
    private List<GameObject> _AllTiles;
    private GameObject _currentplayer;
    
    
    private void Awake()
    {
        Data = Resources.Load<SO_TileManager>("SO_TileManager");
        
        GameManager.instance.TileManager = this;
        _AllTiles = new List<GameObject>();
    }
    
    private void Start()
    {
        _currentplayer = GameManager.instance.playerManager._CurrentPlayerController.gameObject;
        GenerateStartTile();
        
        while (_previousTile.transform.position.z < Data._distanceoOfGeneration) {
            GenerateTile(UnityEngine.Random.Range(0, Data._tiles.Length));
        } 
    }

    private void Update()
    {
        if (_previousTile.transform.position.z <_currentplayer.transform.position.z + Data._distanceoOfGeneration) {
            GenerateTile(UnityEngine.Random.Range(0, Data._tiles.Length));
        }
    }
    
    private void GenerateStartTile() 
    {
        if (Data._startTile.Length <= 0) {
            Debug.LogError("No start tile found in the tile manager", this);
            return;
        }

        Vector3 pos = Vector3.zero;
       
        for (int i = 0; i < Data._startTile.Length; i++) {
            if (i != 0) pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z; // Don't Need for the First tile 
            pos.z += Data._startTile[i].TilePrefab.GetComponentInChildren<Renderer>().bounds.extents.z;
            _previousTile = Instantiate<GameObject>(Data._startTile[i].TilePrefab, pos, Quaternion.identity, transform);
            _AllTiles.Add(_previousTile);
        }
    }
    
    private void GenerateTile( int index)
    {
        Vector3 pos = _previousTile.transform.position;
        pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z;
        pos.z += Data._tiles[index].TilePrefab.GetComponentInChildren<Renderer>().bounds.extents.z;
        _previousTile = Instantiate<GameObject>(Data._tiles[index].TilePrefab, pos, Quaternion.identity, transform);
        _AllTiles.Add(_previousTile);
    }
    
}

[Serializable]
public struct Tile
{
    public GameObject TilePrefab;
}