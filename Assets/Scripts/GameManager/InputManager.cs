using System;
using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    private  List<LeanFinger> _allFingers = new List<LeanFinger>();
    private  List<LeanFinger> _filteredFingers = new List<LeanFinger>();
    
    public bool IsFingerOnScreen(bool CountFingerOverGUI = false)
    {
        _filteredFingers.Clear();
        
        foreach (var finger in _allFingers) {
            
            // Skip fingers that Start over any GUI elements ?
            if (CountFingerOverGUI && finger.StartedOverGui) {
               continue;
            }
            
            _filteredFingers.Add(finger);
        }
        
        return _filteredFingers.Count > 0;
    }
    
    public int GetFingerCount(bool CountFingerOverGUI = false)
    {
        _filteredFingers.Clear();
        
        foreach (var finger in _allFingers) {
            
            // Skip fingers that Start over any GUI elements ?
            if (CountFingerOverGUI && finger.StartedOverGui) {
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
            GameManager.Instance.actionManager.LastFingerUp(finger);
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        _allFingers.Add(finger);
        if (GetFingerCount(true) == 0) {
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
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }
}
