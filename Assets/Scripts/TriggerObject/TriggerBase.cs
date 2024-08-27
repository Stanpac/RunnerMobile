using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class TriggerBase : MonoBehaviour
{
    private BoxCollider _boxCollider;
    private bool _triggered;
    
    [SerializeField][BoxGroup("Parameters")]
    private bool _canBeTriggerMultipleTime = false;
    
    [SerializeField][BoxGroup("layer")]
    private bool _useLayer;
    
    [SerializeField][BoxGroup("layer")][ShowIf("_useLayer")]
    private LayerMask _collisionLayer;
    
    [SerializeField][BoxGroup("Tag")]
    private bool _useTag;
    
    [SerializeField][BoxGroup("Tag")][ShowIf("_useTag")]
    private string _tagToCompare;
    
    [SerializeField][BoxGroup("Debug")]
    private Color _debugColor = Color.red;
    
    [SerializeField][BoxGroup("Event")]
    private bool _triggerEvent;
    
    [Space(10)]
    [SerializeField][BoxGroup("Event")][ShowIf("_triggerEvent")]
    private UnityEvent _trigger;
    
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }
    
    protected virtual void Behavior(Collider other)
    {
        Debug.LogFormat("{0} is Triggered by {1}", gameObject.name, other.name);
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (!_canBeTriggerMultipleTime && _triggered) return;
        
        // If we use layer and the layer is not the one we want to compare
        if ( _useLayer && (_collisionLayer.value & (1 << other.transform.gameObject.layer)) == 0) {
            return;
        }
        
        // If we use tag and the tag is different from the one we want to compare
        if ( _useTag && other.transform.gameObject.tag != _tagToCompare) {
            return;
        }
        
        Behavior(other);
        _triggered = true;
        
        if (_triggerEvent)
            _trigger?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = _debugColor;
        Gizmos.DrawWireCube(transform.position, _boxCollider.size);
    }
#endif
}
