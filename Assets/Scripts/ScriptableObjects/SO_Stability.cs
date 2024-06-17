using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_Stability", menuName = "ScriptableObjects/Stability", order = 0)]
    public class SO_Stability : ScriptableObject
    {
        [SerializeField, BoxGroup("Regen")]
        [Tooltip(" The rate in sec at which the stability of the player regenerates")]
        public float stabilityRegen = 0.1f;
        
        [SerializeField, BoxGroup("Regen"), MinValue(0)]
        [Tooltip("The time in sec without Change of Stability after what the stability starts to regenerate")]
        public float stabilityRegenStopTime = 1;
        
        [SerializeField, BoxGroup("Instability"), Range(0, 1)]
        [Tooltip(" The minimum Force to apply to the stability to Start the Timer _StabilityRegenStop")]
        public float stabilityForce = 0.2f;
        
        [SerializeField, BoxGroup("Instability"),Range(0, 1)] 
        [Tooltip("The % at which the stability is outside the limit and the player is considered unstable")]
        public float instabilityThreshold = 0.8f;
        
        [SerializeField, BoxGroup("Instability"), Range(0, 1)]
        [Tooltip("The % at which the stability is outside the limit and the player is considered unstable")]
        public float stabilityWeightMultiplicator = 1;
        
    }
    
}