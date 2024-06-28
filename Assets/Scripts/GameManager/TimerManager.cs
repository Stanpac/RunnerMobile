using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is responsible for managing the timers in the game.
public class TimerManager : MonoBehaviour
{
    private readonly Dictionary<string, Coroutine> _coroutines = new Dictionary<string, Coroutine>();
    
    
    private string GenerateKey()
    {
        return Guid.NewGuid().ToString();
    }
    
    public string StartTimer(IEnumerator enumerator)
    {
        if (enumerator == null)
            return null;
                
        string key = GenerateKey();
        Coroutine co = StartCoroutine(enumerator);
        _coroutines[key] = co;
        return key;
    }
    
    public void StopTimer(string key)
    {
        if (key == null) 
            return;
        
        Coroutine coroutine = GetTimer(key);
        if (coroutine != null) {
            StopCoroutine(coroutine);
        } else {
            Debug.LogWarning("No coroutine found with the key: " + key);
        }
        
        _coroutines.Remove(key);
    }
    
    public void StopAllTimers()
    {
        foreach (var coroutine in _coroutines.Values) {
            StopCoroutine(coroutine);
        }
        _coroutines.Clear();
    }
    
    public Coroutine GetTimer(string key)
    {
        if (key == null) 
            return null;
        
        if (_coroutines.ContainsKey(key)) {
            return _coroutines[key];
        }
        
        Debug.LogWarning("No coroutine found with the key: " + key);
        return null;
    }
    
    public bool IsTimerRunning(string key)
    {
        if (key == null) 
            return false;
        
        return _coroutines.ContainsKey(key);
    }
}




