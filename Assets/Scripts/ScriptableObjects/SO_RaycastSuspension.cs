using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_RaycastSuspension", menuName = "ScriptableObjects/RaycastSuspension", order = 0)]
    public class SO_RaycastSuspension : ScriptableObject
    {
        [SerializeField, BoxGroup("Suspension Settings")]
        [Tooltip("The distance from the tire to the ground.")]
        public float suspensionRestDist = 0.4f;
        
        [SerializeField, BoxGroup("Suspension Settings")]
        [Tooltip("The strength of the spring.")]
        public float springStrength = 200f;
        
        [SerializeField, BoxGroup("Suspension Settings")]
        [Tooltip("The damping of the spring.")]
        public float springDamper = 30f;
    
        [SerializeField, BoxGroup("Steering Settings")]
        [Tooltip("The Grip of the tire.")]
        public float tireGripFactor = 0.5f;
        
        [SerializeField, BoxGroup("Steering Settings")]
        [Tooltip(" The mass of the tire.")]
        public float tireMass = 1f;
        
    }
}