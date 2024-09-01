using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private EventInstance _instance;

    [SerializeField] private EventReference _globalMusiqueRef;
    
    void Start() 
    { 
        // get the instance 
        _instance = FMODUnity.RuntimeManager.CreateInstance(_globalMusiqueRef);
        
        FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("Intro", "true"); 
        // start play the instance 
        _instance.start();
    }

    private void OnDestroy()
    {
        // Libere le memoire 
        _instance.release();
    }
}
