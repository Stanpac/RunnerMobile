using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "RoadTrip/Tile/TileCard")]
public class TileCard : ScriptableObject
{
    public GameObject _roadTilePrefab;
    public int _creditsCount;
    public float _weight;
    
    // Spawn a tile at the given position and rotation 
    protected void Spawn(Vector3 position, Quaternion rotation, ref TileCard.SpawnResult spawnResult)
    {
        GameObject gameObject = Instantiate<GameObject>(_roadTilePrefab, position, rotation);
        spawnResult.spawnedInstance = gameObject;
        spawnResult.success = true;
    }
    
    // Request to spawn a tile at the given position and rotation 
    public SpawnResult DoSpawn(Vector3 position, Quaternion rotation)
    {
        SpawnResult spawnResult = new SpawnResult() {
            position = position,
            rotation = rotation
        };
        
        Spawn(position, rotation, ref spawnResult);
        return spawnResult;
    }
    
    // Struct to store the result of the spawn request 
    public struct SpawnResult
    {
        public GameObject spawnedInstance;
        public Vector3 position;
        public Quaternion rotation;
        public bool success;
    }
}
