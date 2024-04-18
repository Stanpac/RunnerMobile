using System;
using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public LeanTouch _leanTouch { get; private set; }
    
    [NonSerialized]
    private List<LeanFinger> fingers = new List<LeanFinger>();
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
        fingers.Remove(finger);
        if (fingers.Count == 0) {
            GameManager.instance.actionManager.LastFingerUp();
        }
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        if (finger.StartedOverGui) return;
        if (fingers.Count == 0) {
            GameManager.instance.actionManager.FirstFingerDown();
        }
        fingers.Add(finger);
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        throw new NotImplementedException();
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
