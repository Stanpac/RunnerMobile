using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "RoadTrip/Tiles/TileCard")]
public class TileCard : ScriptableObject
{
    [SerializeField]
    private GameObject _prefab;
    
    [SerializeField]
    private int _creditsCount;
    
    [SerializeField]
    private float _weight;
    
    public GameObject Prefab => _prefab;
    
    public int CreditsCount => _creditsCount;
    
    public float Weight => _weight;
    
    // Spawn a tile at the given position and rotation 
    protected void Spawn(Vector3 position, Quaternion rotation, ref TileCard.SpawnResult spawnResult, Transform parent = null)
    {
        GameObject gameObject = Instantiate<GameObject>(_prefab, position, rotation, parent);
        GameManager.Instance.TileSpawned();
        spawnResult.position = position;
        spawnResult.rotation = rotation;
        spawnResult.spawnedInstance = gameObject;
        spawnResult.success = true;
    }
    
    // Request to spawn a tile at the given position and rotation 
    public SpawnResult DoSpawn(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        SpawnResult spawnResult = new SpawnResult();
        Spawn(position, rotation, ref spawnResult, parent);
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
