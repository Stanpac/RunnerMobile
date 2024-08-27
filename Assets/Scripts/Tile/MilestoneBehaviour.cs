using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;


[RequireComponent(typeof(SplineContainer))]
public class MilestoneBehaviour : MonoBehaviour
{
    // TODO Resolve Problem at the end of the Milestone 
    
    [SerializeField][BoxGroup("Collider")][ReadOnly]
    private BoxCollider _collider;
    
    [SerializeField][BoxGroup("Collider Settings")]
    private float _colliderSizeY = 4;
    [SerializeField][BoxGroup("Collider Settings")]
    private float _colliderSizeZ = 1;
    
    [SerializeField][BoxGroup("Spline Settings")]
    private float _speed = 10;
    
    [SerializeField][BoxGroup("Spline Settings")][Min(0)][MaxValue(1)]
    [Tooltip("normalized time on the spline to gain the luggage")]
    private float _timeGainluggage = 0.5f;
    
    [SerializeField][BoxGroup("Luggage")]
    private int _nbrOfLuggageToGenerate = 3;
    
    // private references to certain components needed
    private SplineContainer _splineContainer = null;
    private SplineAnimate _splineAnimate = null;
    private CarController _carController = null;
    private LuggageHandler _luggageHandler = null;
    
    // Contains the luggages the milestone will give to the player
    Luggage[] _luggages;
    
    private bool _onMilestone = false;
    private bool _luggageGain = false;
    private void Awake()
    {
        if (_collider == null) {
            SpawnCollider();
        }
        
        TryGetComponent(out _splineContainer);
    }

    private void Start()
    {
        // Generate the luggages for the milestone
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
        if (_luggageHandler == null) {
            Debug.LogErrorFormat("No LuggageHandler found in the object {0}", _carController.gameObject.name);
            return;
        }
        
        EnterTheMileStone();
    }

    private void Update()
    {
        if (!_onMilestone) return;
        
        if (_splineAnimate) {
            if (_splineAnimate.NormalizedTime >= _timeGainluggage && !_luggageGain) {
                GainTheLuggage();
            }
            
            if (_splineAnimate.NormalizedTime >= 0.99) {
                ExitTheMilestone();
            }
        }
    }

    private void EnterTheMileStone()
    {
        _carController?.StopMovement(true);
        GameManager.Instance.ActionManager.InvokeMilestoneEvent(_luggageHandler.LuggageLibrary.GetTotalLuggageCount());
        GameManager.Instance.ChangeCam(GameManager.ECamToUse.MileStone);
        SpawnSplinePoint(_carController.transform.position, _carController.transform.rotation);
        StartFollowSpline();
        GameManager.Instance.CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
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
        
        if (_splineAnimate == null) {
            Debug.LogErrorFormat("No SplineAnimate found in the object {0}", _carController.gameObject.name);
            return;
        }
        
        _splineAnimate.Container = _splineContainer;
        _splineAnimate.AnimationMethod = SplineAnimate.Method.Time;
        _splineAnimate.Duration = _speed;
        _splineAnimate.Loop = SplineAnimate.LoopMode.Once;
        _splineAnimate.Restart(true);
        _onMilestone = true;
    }
    
    private void GainTheLuggage()
    {
        _luggageGain = true;
        _splineAnimate?.Pause();
        foreach (var luggage in _luggages) {
            _luggageHandler.AddLuggage(luggage, 1);
        }
        // TODO : Play Animation for gain of luggages
        _splineAnimate?.Play();
        GameManager.Instance.ChangeCam(GameManager.ECamToUse.Main);
    }
    
    private void ExitTheMilestone()
    {
        _splineAnimate?.Pause();
        _carController?.StartMovement();
        _onMilestone = false;
        GameManager.Instance.CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }
    
    // Generate the collider for the milestone with good position and size
    [Button]
    private void SpawnCollider()
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
        
        if (_collider == null) {
            _collider = gameObject.AddComponent<BoxCollider>();
            _collider.isTrigger = true;
            _collider.size = size;
            _collider.center = -pos;
        }
    }
}
