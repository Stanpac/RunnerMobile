using System;
using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    //public LeanTouch _leanTouch { get; private set; }
    
    public bool IsFingerOnScreen(bool CountFingerOverGUI = false)
    {
        if (CountFingerOverGUI) {
            return LeanTouch.Fingers.Count > 0;
        }
        
        // return True if there is a finger on the screen that have not start on a GUI element
        return LeanTouch.GetFingers(true, false, 0, false ).Count > 0;
    }
   
    private void Awake()
    {
        // Find Lean Touch in the scene or add it if it doesn't exist
        /*_leanTouch = gameObject.GetComponent<LeanTouch>();
        if (_leanTouch == null) {
            _leanTouch = gameObject.AddComponent<LeanTouch>();
        }*/
    }
    
    private void HandleFingerUp(LeanFinger finger)
    {
        if (LeanTouch.Fingers.Count == 0) {
            GameManager.Instance.actionManager.LastFingerUp(finger);
        } 
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        if (LeanTouch.Fingers.Count == 0) {
            GameManager.Instance.actionManager.FirstFingerDown(finger);
        } else {
            GameManager.Instance.actionManager.FingerDown(finger);
        }
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }
    
    private void OnDisable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }
}
