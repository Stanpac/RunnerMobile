using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_RaycastSuspension", menuName = "ScriptableObject/RaycastSuspension", order = 0)]
    public class SO_RaycastSuspension : ScriptableObject
    {
        [BoxGroup("Suspension Settings")]
        public float suspensionRestDist = 0.5f;
        
        [BoxGroup("Suspension Settings")]
        public float springStrength = 100f;
        
        [BoxGroup("Suspension Settings")]
        public float springDamper = 15f;
    
        [BoxGroup("Steering Settings")]
        public float tireGripFactor = 0.5f;
        
        [BoxGroup("Steering Settings")]
        public float tireMass = 10f;
    
        [BoxGroup("Acceleration Settings")]
        public float CarSpeed = 0.0f;
        
        [BoxGroup("Acceleration Settings")]
        public float carTopSpeed = 10f;
        
        [BoxGroup("Acceleration Settings")]
        public AnimationCurve powerCurve;
    }
}