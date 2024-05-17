using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_Stability", menuName = "ScriptableObject/Stability", order = 0)]
    public class SO_Stability : ScriptableObject
    {
        [Tooltip(" The rate in sec at which the stability of the player regenerates")]
        [SerializeField, BoxGroup("Regen")]
        public float _stabilityRegen = 0.1f;
        
        [Tooltip("The time in sec without Change of Stability after what the stability starts to regenerate")]
        [SerializeField, BoxGroup("Regen"), MinValue(0)]
        public float _stabilityRegenStopTime = 1;
        
        [Tooltip(" The minimum Force to apply to the stability to Start the Timer _StabilityRegenStop")]
        [SerializeField, BoxGroup("Instability"), Range(0, 1)]
        public float _stabilityForce = 0.2f;
        
        [Tooltip("The % at which the stability is outside the limit and the player is considered unstable")]
        [SerializeField, BoxGroup("Instability"),Range(0, 1)] 
        public float _instabilityThreshold = 0.8f;
    }
    
}