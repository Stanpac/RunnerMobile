using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager 
{
    public CarController CurrentCarController { get; private set; }
    public CarController CarPrefab { get; set; }
    
    public void InstantiatePlayer(Vector3 position, Quaternion rotation) 
    {
        if (CarPrefab == null) {
            Debug.LogError("Player Prefab is not set");
            return;
        }
        
        if (CurrentCarController != null) {
            GameObject.Destroy(CurrentCarController.gameObject);
        }
        
        CurrentCarController = GameObject.Instantiate<GameObject>(CarPrefab.gameObject, position, rotation).GetComponent<CarController>();
        GameManager.Instance.SetParametersForCamera(CurrentCarController.transform, CurrentCarController.transform);
    }
    
    public void GiveStartImpulsionToPlayer(Vector3 direction, float force)
    {
        if (CurrentCarController == null) {
            Debug.LogError("No Player to give impulsion");
            return;
        }
        
        CurrentCarController.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
    }
} 
