using System;
using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public LeanTouch _leanTouch { get; private set; }
    
    [NonSerialized]
    private List<LeanFinger> _fingers = new List<LeanFinger>();
    private void Awake()
    {
        // Find Lean Touch in the scene or add it if it doesn't exist
        _leanTouch = gameObject.GetComponent<LeanTouch>();
        if (_leanTouch == null) {
            _leanTouch = gameObject.AddComponent<LeanTouch>();
        }
    }
    
    private void HandleFingerUp(LeanFinger finger)
    {
        _fingers.Remove(finger);
        if (_fingers.Count == 0) {
            GameManager._instance.actionManager.LastFingerUp(finger);
        } 
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        if (_fingers.Count == 0) {
            GameManager._instance.actionManager.FirstFingerDown(finger);
        } else {
            GameManager._instance.actionManager.FingerDown(finger);
        }
        _fingers.Add(finger);
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        // handle finger update
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }
    
    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }
}
