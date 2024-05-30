using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Lean.Touch;
using ScriptableObjects;
using UnityEngine;


/*
 * This script is responsible for generating the tiles
 * And manage there movement
 */
// TODO : Move all the variable in a scriptable object
public class TileManager : MonoBehaviour
{
    [SerializeField]
    private SO_TileManager _tileManager;
    
    public Tile[] _tiles;
    public float _distanceoOfGeneration;
    
    public float _speed;
    
    [Tag] 
    public String _groundTag;
    
    public float AngleMaxrotation = 45;
    public float TimeForMaxRotation = 1f;
    
    // Private variable
    //private Vector3 _groundDirection;
    //private float RotationAngle = 0;
    
    private GameObject _previousTile;
    private List<GameObject> _AllTiles;
    
    //private bool FingerOnScreen = false;
    //private LeanFinger _Currentfinger;

    private void Awake()
    {
        GameManager.instance.TileManager = this;
        _AllTiles = new List<GameObject>();
        
        // Event  
        GameManager.instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager.instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager.instance.actionManager.OnLastFingerUp += OnLastFingerUp;
        RotationAngle = 0;
    }
    
    private void Start()
    {
        float Offset =  _tiles[0].TilePrefab.GetComponentInChildren<Renderer>().bounds.extents.y + 0.1f;
        GenerateTile( GameManager.instance.playerManager._CurrentPlayerController.transform.position + new Vector3(0,-Offset, 0), 0);
        
        while (_previousTile.transform.position.z < _distanceoOfGeneration) {
            Vector3 pos = _previousTile.transform.position;
            pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z;
            GenerateTile(pos , UnityEngine.Random.Range(1, _tiles.Length));
        } 
    }

    private void Update()
    {
        _groundDirection = CalculateMovementDirection();
        _groundDirection = CalculateRotation();
        MoveTheTiles();
        GameManager.instance.playerManager._CurrentPlayerController.GetComponent<PlayerController>().SetRotation(Quaternion.LookRotation(-_groundDirection));
        
        if (_previousTile.transform.position.z < _distanceoOfGeneration) {
            Vector3 pos = _previousTile.transform.position;
            pos.z += _previousTile.GetComponentInChildren<Renderer>().bounds.extents.z;
            GenerateTile(pos, UnityEngine.Random.Range(1, _tiles.Length));
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
        Vector3 _direction =  GameManager.instance.playerManager._CurrentPlayerController.transform.forward * Time.deltaTime;
        
        RaycastHit hit;
        if (Physics.Raycast( GameManager.instance.playerManager._CurrentPlayerController.transform.position + Vector3.up, -transform.up  + _direction, out hit, 10f)) {
            if (hit.collider.gameObject.CompareTag(_groundTag))  {
                _direction = Vector3.ProjectOnPlane(_direction, hit.normal).normalized;
            } else {
                Debug.LogError("Ground not found", this);
            }
        }

        return -_direction;
    }
    
    private Vector3 CalculateRotation()
    {
       float rotation = RotationAngle;
       
       if (FingerOnScreen) {
           if (_Currentfinger.ScreenPosition.x > Screen.width / 2) {
               rotation =  Mathf.Clamp(rotation + Time.deltaTime / TimeForMaxRotation * AngleMaxrotation, -AngleMaxrotation, AngleMaxrotation);
           } else {
               rotation =  Mathf.Clamp(rotation - Time.deltaTime / TimeForMaxRotation * AngleMaxrotation, -AngleMaxrotation, AngleMaxrotation);
           }
       } 
       
       RotationAngle = rotation;
       return  Quaternion.AngleAxis(rotation, Vector3.up) * _groundDirection;
    }
    
    private void GenerateTile(Vector3 pos, int index)
    {
        Vector3 position = pos;
        position.z += _tiles[index].TilePrefab.GetComponentInChildren<Renderer>().bounds.extents.z;
        _previousTile = Instantiate<GameObject>(_tiles[index].TilePrefab, position, Quaternion.identity, transform);
        _AllTiles.Add(_previousTile);
    }
    
    // private void OnFingerDown(LeanFinger finger)
    // {
    //     FingerOnScreen = true;
    //     if (_Currentfinger == null || !_Currentfinger.Set) {
    //         _Currentfinger = finger;
    //     }
    // }
    //
    // private void OnLastFingerUp(LeanFinger obj)
    // {
    //    FingerOnScreen = false;
    //    _Currentfinger = null;
    // }
}

[Serializable]
public struct Tile
{
    public GameObject TilePrefab;
}