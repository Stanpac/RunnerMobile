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
    protected BoxCollider BoxCollider;
    protected bool Triggered;
    protected GameObject TriggeredBy;
    
    [SerializeField][BoxGroup("Parameters")]
    protected bool _canBeTriggerMultipleTime = false;
    
    [SerializeField][BoxGroup("layer")]
    protected bool _useLayer;
    
    [SerializeField][BoxGroup("layer")][ShowIf("_useLayer")]
    protected LayerMask _collisionLayer;
    
    [SerializeField][BoxGroup("Tag")]
    protected bool _useTag;
    
    [SerializeField][BoxGroup("Tag")][ShowIf("_useTag")][Tag]
    protected string _tagToCompare;
    
    [SerializeField][BoxGroup("Debug")]
    protected Color _debugColor = Color.red;
    
    [SerializeField][BoxGroup("Event")]
    protected bool _triggerEvent;
    
    [Space(10)]
    [SerializeField][BoxGroup("Event")][ShowIf("_triggerEvent")]
    protected UnityEvent _trigger;

    protected virtual void Reset()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.isTrigger = true;
    }

    protected virtual void Awake() 
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.isTrigger = true;
    }
    
    protected virtual void Behavior()
    {
        Debug.LogFormat("{0} is Triggered by {1}", gameObject.name, TriggeredBy.name);
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (!_canBeTriggerMultipleTime && Triggered) return;
        
        // If we use layer and the layer is not the one we want to compare
        if ( _useLayer && (_collisionLayer.value & (1 << other.transform.gameObject.layer)) == 0) {
            return;
        }
        
        // If we use tag and the tag is different from the one we want to compare
        if ( _useTag && other.transform.gameObject.tag != _tagToCompare) {
            return;
        }
        
        TriggeredBy = other.gameObject;
        Triggered = true;
        
        if (_triggerEvent)
            _trigger?.Invoke();
        
        Behavior();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (BoxCollider == null) {
            BoxCollider = GetComponent<BoxCollider>();
        }
        
        Gizmos.color = _debugColor;
        Gizmos.DrawWireCube(transform.position, BoxCollider.size);
    }
#endif
}
