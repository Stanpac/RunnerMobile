using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_PlayerController", menuName = "ScriptableObjects/PlayerController", order = 0)]
    public class SO_PlayerController : ScriptableObject
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
        
        [SerializeField, BoxGroup("Rotation Settings")]
        [Tooltip("The max rotation of the car.")]
        public float angleMaxRotation = 45;
        
        [SerializeField, BoxGroup("Rotation Settings")]
        [Tooltip("The min rotation of the car that impact stability. (need to be lower than angleMaxRotation)")]
        public float angleMinforStabilityImpact = 30;
        
        [SerializeField, BoxGroup("Rotation Settings")]
        [Tooltip("The time for the wheels to reach AngleMaxrotation")]
        public float timeForMaxRotation = 1f;
    }
}