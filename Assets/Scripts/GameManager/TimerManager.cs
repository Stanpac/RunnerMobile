using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is responsible for managing the timers in the game.
public class TimerManager : MonoBehaviour
{
    private Dictionary<string, Coroutine> _coroutines = new Dictionary<string, Coroutine>();
    
    // TODO : Add a way to stop all the timers when the game is over
    // TODO : remove mono behaviors of this Script, need to add monobehaviors context to functions for handle the coroutines
    private string GenerateKey()
    {
        return Guid.NewGuid().ToString();
    }
    
    public string StartTimer(IEnumerator coroutine)
    {
        string key = GenerateKey();
        Coroutine co = StartCoroutine(coroutine);
        _coroutines[key] = co;
        return key;
    }
    
    public void StopTimer(string key)
    {
        if (_coroutines.ContainsKey(key))
        {
            StopCoroutine(_coroutines[key]);
            _coroutines.Remove(key);
        } else {
            Debug.LogWarning("No coroutine found with the key: " + key);
        }
    }
    
    public void StopAllTimers()
    {
        foreach (var coroutine in _coroutines.Values)
        {
            StopCoroutine(coroutine);
        }
        _coroutines.Clear();
    }
    
    public Coroutine GetTimer(string key)
    {
        if (_coroutines.ContainsKey(key))
        {
            return _coroutines[key];
        }
        Debug.LogWarning("No coroutine found with the key: " + key);
        return null;
    }
    
    public bool IsTimerRunning(string key)
    {
        if (key == null) {
            return false;
        }
        return _coroutines.ContainsKey(key);
    }
}




