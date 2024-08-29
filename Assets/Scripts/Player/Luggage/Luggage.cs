using System;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(menuName = "RoadTrip/Luggages/Luggage")]
public class Luggage : ScriptableObject
{
    [SerializeField]
    private GameObject _prefab;
    
    [SerializeField]
    private string _name;
    
    [SerializeField]
    private float _weight;
    
    [SerializeField]
    private LuggageStability _stability;
    
    [SerializeField]
    private Texture2D _icon;
    
    // Voicelines when the luggage is picked up
    // Voicelines when the luggage is dropped
    
    public GameObject Prefab => _prefab;
    public string Name => _name;
    public float Weight => _weight;
    public LuggageStability Stability => _stability;
    
    // Spawn a tile at the given position and rotation 
    protected void Spawn(Vector3 position, Quaternion rotation, ref Luggage.SpawnResult spawnResult, Transform parent = null)
    {
        GameObject gameObject = Instantiate<GameObject>(_prefab, position, rotation, parent);
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
    
    public static void SortByStability(ref Luggage[] luggages)
    {
        Array.Sort(luggages, (a, b) => a.Stability.CompareTo(b.Stability));
    }
    
    // Struct to store the result of the spawn request 
    public struct SpawnResult
    {
        public GameObject spawnedInstance;
        public Vector3 position;
        public Quaternion rotation;
        public bool success;
    }
    
    public enum LuggageStability
    {
        low,
        medium,
        high
    }
}

