using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] 
    public PlayerController _CurrentPlayerController;
    
    
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
        
        if (_CurrentPlayerController != null) {
            Destroy(_CurrentPlayerController.gameObject);
        }
        
        _CurrentPlayerController = Instantiate<GameObject>(_currentPlayerPrefab, position, rotation).GetComponent<PlayerController>();
        GameManager.instance.camera.target = _CurrentPlayerController.transform;
    }
} 
