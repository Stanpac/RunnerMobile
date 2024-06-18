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
    
    [SerializeField, BoxGroup("Debug")]
    private Color _debugColor = Color.red;
    
    [SerializeField, BoxGroup("Event")]
    private bool _triggerEvent;
    
    [Space(10)]
    [SerializeField, BoxGroup("Event"), ShowIf("_triggerEvent")]
    private UnityEvent OnTrigger;

    private void Reset()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }
    protected virtual void behavior()
    {
       Debug.Log("Triggered");
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        _triggered = true;
        
        behavior();
        
        if (_triggerEvent)
            OnTrigger?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = _debugColor;
        Gizmos.DrawWireCube(transform.position, _boxCollider.size);
    }
#endif
}
