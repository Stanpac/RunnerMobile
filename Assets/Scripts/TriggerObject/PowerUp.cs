
using System;
using NaughtyAttributes;
using NaughtyAttributes.Test;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PowerUp : TriggerBase
{
    [SerializeField][BoxGroup("PowerUp")]
    private float _powerUpDuration = 5;
    
    [SerializeField][BoxGroup("PowerUp")]
    private float _stabilityMultiplicator = 0.7f;
    
    [SerializeField][BoxGroup("Animation")]
    private bool _useAnimation;
    
    [SerializeField][BoxGroup("Animation")][ShowIf("_useAnimation")]
    private Animation _animation;
    
    [SerializeField][BoxGroup("Animation")][ShowIf("_useAnimation")][Min(0), MaxValue(1)]
    [Tooltip("% of the animation to trigger the event")]
    public float _eventTime;
    
    [SerializeField][BoxGroup("Animation")][ShowIf("_useAnimation")]
    public UnityEvent _animationEvent;
    
    
    protected override void Reset()
    {
        base.Reset();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Behavior();
    }

    protected override void Behavior()
    {
        if (TriggeredBy != null) {
            CarController carController = TriggeredBy.GetComponent<CarController>();
            if (carController != null) {
                carController.ApplyPowerUp(_powerUpDuration, _stabilityMultiplicator);
            }
        }
        
        if (_useAnimation && _animation != null && _animation.clip != null) {
            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.time = _animation.clip.length * _eventTime;
            animationEvent.functionName = "CallAnimationEvent";
            _animation.clip.AddEvent(animationEvent);
            
            if (!_canBeTriggerMultipleTime) {
                AnimationEvent destroyEvent = new AnimationEvent();
                destroyEvent.time = _animation.clip.length;
                destroyEvent.functionName = "DestroyPowerUp";
                _animation.clip.AddEvent(destroyEvent);
            }
            _animation.Play();
        } else {
            if (!_canBeTriggerMultipleTime) {
                Destroy(gameObject);   
            }
        }
    }
    
    private void CallAnimationEvent()
    {
        _animationEvent.Invoke();
    }

    public void DestroyPowerUp()
    {
        Destroy(gameObject);
    }
}
