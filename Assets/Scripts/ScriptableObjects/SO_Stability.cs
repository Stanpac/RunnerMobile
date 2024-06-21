using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_Stability", menuName = "ScriptableObjects/Stability", order = 0)]
    public class SO_Stability : ScriptableObject
    {
        [SerializeField, BoxGroup("Instability"),Range(0, 1)] 
        [Tooltip("The % at which the stability is outside the limit and the player is considered unstable")]
        public float instabilityThreshold = 0.8f;
        
        [SerializeField, BoxGroup("Instability")] 
        [Tooltip("The % at which the stability is outside the limit and the player is considered unstable")]
        public AnimationCurve instabilityInputTimeCurve;
        
        [SerializeField, BoxGroup("Instability"), MinValue(0)] 
        [Tooltip("time in seconds to reach the max instability based on the Curve Value, 0 means instant")]
        public float timeForReachMaxinstability = 2;
        
    }
    
}