using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager 
{
    public CarController _currentCarController { get; private set; }
    public CarController _carPrefab { get; set; }
    
    public void InstantiatePlayer(Vector3 position, Quaternion rotation) 
    {
        if (_carPrefab == null) {
            Debug.LogError("Player Prefab is not set");
            return;
        }
        
        if (_currentCarController != null) {
            GameObject.Destroy(_currentCarController.gameObject);
        }
        
        _currentCarController = GameObject.Instantiate<GameObject>(_carPrefab.gameObject, position, rotation).GetComponent<CarController>();
        GameManager.Instance._virtualCamera.LookAt = _currentCarController.transform;
        GameManager.Instance._virtualCamera.Follow = _currentCarController.transform;
    }
    
    public void GiveStartImpulsionToPlayer(Vector3 direction, float force)
    {
        if (_currentCarController == null) {
            Debug.LogError("No Player to give impulsion");
            return;
        }
        
        _currentCarController.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
    }
} 
