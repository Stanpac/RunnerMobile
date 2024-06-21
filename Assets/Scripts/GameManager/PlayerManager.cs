using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager : MonoBehaviour
{
    [FormerlySerializedAs("_currentPlayerController")] [SerializeField] 
    public CarController _currentCarController;
    
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
        
        if (_currentCarController != null) {
            Destroy(_currentCarController.gameObject);
        }
        
        _currentCarController = Instantiate<GameObject>(_currentPlayerPrefab, position, rotation).GetComponent<CarController>();
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
