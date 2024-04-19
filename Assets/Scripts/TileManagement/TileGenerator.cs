using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public struct Tile
{
    public GameObject TilePrefab;
}

/*
 * This script is responsible for generating the tiles
 * And manage there movement
 */
// TODO : Move all the variable in a scriptable object
public class TileGenerator : MonoBehaviour
{
    /*------------------- public / SerializeField variable -------------------*/
    
    public Tile[] _tiles;
    public float _distanceoOfGeneration;
    
    public float _speed;
    [Tag] public String _groundTag;
    
    /*------------------- End public / SerializeField variable -------------------*/
    /*------------------- Private Variables -------------------*/
    
    private Vector3 _PlayerPosition = new Vector3(0, 0, 0);
    private Quaternion _PlayerRotation = Quaternion.identity;
    private Vector3 _groundDirection;
    private GameObject _previousTile;
    private GameObject _CurrentPlayerInstance;
    private List<GameObject> _AllTiles;
    
    /*------------------- End Private Variables -------------------*/

    private void Awake()
    {
        _CurrentPlayerInstance = Instantiate<GameObject>(GameManager.instance._currentPlayerPrefab, _PlayerPosition, _PlayerRotation);
        GameManager.instance.camera.target = _CurrentPlayerInstance.transform;
        _AllTiles = new List<GameObject>();
    }
    
    private void Start()
    {
        float Offset = _CurrentPlayerInstance.GetComponentInChildren<Renderer>().bounds.extents.y + _tiles[0].TilePrefab.GetComponentInChildren<Renderer>().bounds.extents.y + 0.1f;
        GenerateTile( _PlayerPosition + new Vector3(0,-Offset, 0), 0);
        
        while (_previousTile.transform.position.z < _distanceoOfGeneration) {
            Vector3 pos = _previousTile.transform.position;
            pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z;
            GenerateTile(pos , UnityEngine.Random.Range(0, _tiles.Length));
        } 
    }

    private void Update()
    {
        _groundDirection = CalculateMovementDirection();
        MoveTheTiles();
        
        if (_previousTile.transform.position.z < _distanceoOfGeneration) {
            Vector3 pos = _previousTile.transform.position;
            pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z;
            GenerateTile(pos, UnityEngine.Random.Range(0, _tiles.Length));
        }
    }
    
    private void MoveTheTiles()
    {
        if (_AllTiles == null) return;
        for (int i = 0; i < _AllTiles.Count; i++)
        {
            _AllTiles[i].transform.position += _groundDirection * (_speed / 100);
        }
    }
    
    private Vector3 CalculateMovementDirection()
    {
        Vector3 _direction = _CurrentPlayerInstance.transform.forward * Time.deltaTime;
        
        RaycastHit hit;
        if (Physics.Raycast(_CurrentPlayerInstance.transform.position, -transform.up + _direction, out hit, 10f)) {
            if (hit.collider.gameObject.CompareTag(_groundTag))  {
                _direction = Vector3.ProjectOnPlane(_direction, hit.normal).normalized;
            } else {
                Debug.LogError("Ground not found", this);
            }
        }

        return -_direction;
    }
    
    private void GenerateTile(Vector3 pos, int index)
    {
        Vector3 position = pos;
        position.z += _tiles[index].TilePrefab.GetComponentInChildren<Renderer>().bounds.extents.z;
        _previousTile = Instantiate<GameObject>(_tiles[index].TilePrefab, position, Quaternion.identity, transform);
        _AllTiles.Add(_previousTile);
    }
}
