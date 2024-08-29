using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField][BoxGroup("SpawPoints")] 
    private Transform[] _spawnPoints;
    
    [SerializeField][BoxGroup("Parameters")]
    private float _spawnChance = 0.1f;
    
    [SerializeField][BoxGroup("PowerUps")]
    private GameObject[] _powerUps;
    
    void Start()
    {
        if (GameManager.Instance == null && !GameManager.Instance.CanSpawnPowerUp) {
            return;
        }
        
        if (_spawnPoints.Length <= 0 || _powerUps.Length <= 0) {
            return;
        }
        
        foreach (Transform spawnPoint in _spawnPoints) {
            if (Random.value < _spawnChance) {
                Instantiate(_powerUps[Random.Range(0, _powerUps.Length)], spawnPoint.position, spawnPoint.rotation, transform);
                GameManager.Instance.PowerUpSpawned();
                return;
            }
        }
    }
}
