using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private List<Coroutine> _coroutines = new List<Coroutine>();
    
    public bool StartTimer(IEnumerator DynDelegate, ref FTimerHandler timerHandler)
    {   
        if (timerHandler.Initialized) {
            return false;
        }
        {
            StopCoroutine(coroutineHandler);
        }
        if ( _coroutines.Contains(coroutineHandler)) {
            StopCoroutine(coroutineHandler);
        }
        _coroutines.Add(StartCoroutine(DynDelegate));
    }
}


public struct FTimerHandler
{
    private Coroutine _coroutineHandler;
    private bool _running;
    private bool _initialized;
    
    public FTimerHandler(Coroutine coroutineHandler, bool running, bool initialized)
    {
        this._coroutineHandler = coroutineHandler;
        this._running = running;
        this._initialized = initialized;
    }
    
    public Coroutine CoroutineHandler => _coroutineHandler;
    
    public bool Running => _running;
    
    public bool Initialized => _initialized;
}


