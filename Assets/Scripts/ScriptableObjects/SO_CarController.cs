﻿using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_CarController", menuName = "ScriptableObjects/CarController", order = 0)]
    public class SO_CarController : ScriptableObject
    {
        [SerializeField, BoxGroup("Speed Settings")]
        [Tooltip(" Multiply the speed applied to the car without intefering in the normalisation of the speed.")]
        public float speedFactor = 5f;
        
        [SerializeField, BoxGroup("Speed Settings")]
        [Tooltip("The max Speed of the car.")]
        public float carTopSpeed = 100f;
        
        [SerializeField, BoxGroup("Speed Settings")]
        [Tooltip("% of the carTopSpeed add to the speed according to normalized speed of the car.")]
        public AnimationCurve powerCurve = new AnimationCurve();
        
        [SerializeField, BoxGroup("Rotation Control Settings")]
        [Tooltip("The max rotation of the wheels")]
        public float maxRotation = 45;
        
        [SerializeField, BoxGroup("Rotation Control Settings")]
        [Tooltip("The time for the wheels to reach AngleMaxrotation")]
        public float timeForMaxRotation = 1f;
        
        [SerializeField, BoxGroup("Rotation Control Settings"), MinValue(0), MaxValue(1)]
        [Tooltip("if the car goes past this % of the max rotation , she will start to reach this value in timeForReachTreshold seconde")]
        public float rotationCenterTreshold = 0.4f;
        
        [SerializeField, BoxGroup("Rotation Control Settings")]
        [Tooltip("The time for the wheels to reach rotationCenterTreshold % of the max rotation")]
        public float timeForReachTreshold = 1f;
        
        [SerializeField, BoxGroup("Rotation physics Settings")] 
        [Tooltip("The max angle of the car relative to the World Up Vector")]
        public float angleUpMaxRotation  = 45f;
        
        [SerializeField, BoxGroup("Rotation physics Settings")] 
        [Tooltip(" at which speed the car will rotate to reach the max angleUpMaxRotation")]
        public float speedOfTheRotation = 5f;
    }
}