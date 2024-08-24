using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;


[RequireComponent(typeof(SplineContainer))]
public class MilestoneBehaviour : MonoBehaviour
{
    // TODO : test to now if this works 
    
    [SerializeField][BoxGroup("Colliders")][ReadOnly]
    private BoxCollider _entryCollider;
    [SerializeField][BoxGroup("Colliders")][ReadOnly]
    private BoxCollider _exitCollider;
    
    [SerializeField][BoxGroup("Colliders")]
    private float _colliderSizeY = 4;
    [SerializeField][BoxGroup("Colliders")]
    private float _colliderSizeZ = 1;
    
    private SplineContainer _splineContainer;
    private EMilestoneState _milestoneState = EMilestoneState.Entry;
    
    // Timer keys
    private string _timerFollowSplineKey;
    CarController _carController = null;
    SplineAnimate _splineAnimate = null;
    LuggageHandler _luggageHandler = null;
    
    // luggage
    Luggage[] _luggages;
    
    
    // Parmeters for Spline Lerp
    public float _speed = 10;
    public int _nbrOfLuggageToGenerate = 3;
    
    private void Awake()
    {
        if (_entryCollider == null || _exitCollider == null) {
            SpawnColliders();
        }
        _milestoneState = EMilestoneState.Entry;
        TryGetComponent(out _splineContainer);
    }

    private void Start()
    {
        GenerateLuggages();
    }

    private void GenerateLuggages()
    {
        _luggages = GameManager.Instance.LuggageCategories.PickLuggagesInRandomCategory(_nbrOfLuggageToGenerate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out _carController)) {
            _carController = other.gameObject.GetComponentInParent<CarController>();
            if (_carController == null) {
                Debug.LogErrorFormat("No CarController found in the object {0}", other.gameObject.name);
                return;
            }
        }
        _luggageHandler = _carController.GetComponent<LuggageHandler>();
        switch (_milestoneState) {
            case EMilestoneState.Entry:
                OnEntry();
                break;
            case EMilestoneState.Pause:
                OnPause();
                break;
            case EMilestoneState.Exit:
                OnExit();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnEntry()
    {
        _carController.StopMovement();
        GameManager.Instance.ActionManager.InvokeMilestoneEvent(_luggageHandler.LuggageLibrary.GetTotalLuggageCount());
        // TODO : Start Change of The camera
        SpawnSplinePoint(_carController.transform.position, _carController.transform.rotation);
        StartFollowSpline();
        _milestoneState = EMilestoneState.Pause;
    }
    
    // Spawn a spline point with position and rotation and add it to the spline
    private void SpawnSplinePoint(Vector3 position, quaternion rotation)
    {
        // Set local position
        position -= this.transform.position;
        BezierKnot knot = new BezierKnot(position,10 ,10, rotation);
        _splineContainer.Spline.Insert(0, knot, TangentMode.AutoSmooth);
    }
    
    private void StartFollowSpline()
    {
        if (!_carController.TryGetComponent(out _splineAnimate)) {
            _splineAnimate = _carController.AddComponent<SplineAnimate>();
        }
        _splineAnimate.Container = _splineContainer;
        _splineAnimate.AnimationMethod = SplineAnimate.Method.Time;
        _splineAnimate.Duration = _speed;
        _splineAnimate.Loop = SplineAnimate.LoopMode.Once;
        _splineAnimate.Play();
    }
    
    private void OnPause()
    {
        _splineAnimate?.Pause();
         AddLuggages();
        _splineAnimate?.Play();
        // TODO : Change the camera to the player Camera
    }
    
    private void AddLuggages()
    {
        foreach (var luggage in _luggages) {
            _luggageHandler.AddLuggage(luggage, 1);
        }
        // TODO : Play Animation for gain of luggages
    }
    
    private void OnExit()
    {
        _splineAnimate?.Pause();
        _carController.StartMovement();
    }
    
    // Generate the colliders for the milestone with good position and size
    [Button]
    private void SpawnColliders()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null) {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        
        if (meshRenderer == null) {
            Debug.LogErrorFormat("No MeshRenderer found");
            return;
        }
        
        Vector3 size = meshRenderer.bounds.size;
        size = new Vector3(size.x, _colliderSizeY, _colliderSizeZ);
        
        Vector3 pos = meshRenderer.bounds.center + new Vector3(0, 0, meshRenderer.bounds.size.z / 2);
        
        if (_entryCollider == null) {
            _entryCollider = gameObject.AddComponent<BoxCollider>();
            _entryCollider.isTrigger = true;
            _entryCollider.size = size;
            _entryCollider.center = -pos;
        }
        
        if (_exitCollider == null) {
            _exitCollider = gameObject.AddComponent<BoxCollider>();
            _exitCollider.isTrigger = true;
            _exitCollider.size = size;
            _exitCollider.center = pos;
        }
    }
    
    private enum EMilestoneState
    {
        Entry,
        Pause,
        Exit
    }
}
