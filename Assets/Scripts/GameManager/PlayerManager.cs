using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] 
    public PlayerController _currentPlayerController;
    
    [SerializeField] 
    public GameObject _currentPlayerPrefab { get; set; }
    
    private void Awake()
    {
        
    }
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        
    }
    
    public void InstantiatePlayer(Vector3 position, Quaternion rotation) 
    {
        if (_currentPlayerPrefab == null) {
            Debug.LogError("Player Prefab is not set");
            return;
        }
        
        if (_currentPlayerController != null) {
            Destroy(_currentPlayerController.gameObject);
        }
        
        _currentPlayerController = Instantiate<GameObject>(_currentPlayerPrefab, position, rotation).GetComponent<PlayerController>();
        GameManager._instance._virtualCamera.LookAt = _currentPlayerController.transform;
        GameManager._instance._virtualCamera.Follow = _currentPlayerController.transform;
    }
    
    public void GiveStartImpulsionToPlayer(Vector3 direction, float force)
    {
        if (_currentPlayerController == null) {
            Debug.LogError("No Player to give impulsion");
            return;
        }
        
        _currentPlayerController.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
    }
} 
