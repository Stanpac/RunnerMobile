using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "RoadTrip/ScriptableObjects/ScoreManager")]
    public class SO_ScoreManager : ScriptableObject
    {
        // Action value
        [SerializeField][BoxGroup("Action value")]
        [Tooltip("Acction value for the first milestone")]
        public float _avForNextMileStoneBase = 1000;
        [SerializeField][BoxGroup("Action value")]
        [Tooltip("multiplier apply to the action value when the player is on the road")]
        public float _avRoadScoreMultiplier = 2f;
        
        // MileStone 
        [SerializeField][BoxGroup("MileStone")]
        [Tooltip("Coefficient apply to the Av for the next milestone")]
        public float _avNextMilestoneCoef = 1.1f;
        [SerializeField][BoxGroup("MileStone")]
        [Tooltip("Multiplier of the luggage for the milestone score")]
        public float _milestoneScoreCoef = 2f;
        
        // Coefficient of difficulty
        [SerializeField][BoxGroup("Difficulty")]
        [Tooltip("Base coefficient of difficulty")]
        public float _baseCoefficientDiffiCulty = 2f;
    }
}