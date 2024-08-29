using System;
using Lean.Touch;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class InputManager 
{
    private  List<LeanFinger> _allFingers = new List<LeanFinger>();
    private  List<LeanFinger> _filteredFingers = new List<LeanFinger>();
    
    
    public bool IsFingerOnScreen(bool countFingerOverGUI = false)
    {
        _filteredFingers.Clear();
        
        foreach (var finger in _allFingers) {
            
            // Skip fingers that Start over any GUI elements ?
            if (countFingerOverGUI && finger.StartedOverGui) {
               continue;
            }
            
            _filteredFingers.Add(finger);
        }
        
        Debug.Log(_filteredFingers.Count);
        return _filteredFingers.Count > 0;
    }
    
    public int GetFingerCount(bool countFingerOverGUI = false)
    {
        _filteredFingers.Clear();
        
        foreach (var finger in _allFingers) {
            
            // Skip fingers that Start over any GUI elements ?
            if (countFingerOverGUI && finger.StartedOverGui) {
               continue;
            }
            
            _filteredFingers.Add(finger);
        }
        
        return _filteredFingers.Count;
    }
    
    // here we need to check the ActiveFingers because the LeanTouch.Fingers. Keep the finger more frame than we need
    private void HandleFingerUp(LeanFinger finger)
    {
        _allFingers.Remove(finger);
        if (GetFingerCount(true) == 0)
            GameManager.Instance.ActionManager.InvokeLastFingerUp(finger);
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        _allFingers.Add(finger);
        if (GetFingerCount(true) == 0) {
            GameManager.Instance.ActionManager.InvokeFirstFingerDown(finger);
        } else {
            GameManager.Instance.ActionManager.InvokeFingerDown(finger);
        }
    }
    public InputManager()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }
    
    ~InputManager() 
    {
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }
}
