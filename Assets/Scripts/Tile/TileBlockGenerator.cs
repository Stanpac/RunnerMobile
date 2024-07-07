using System;
using System.Collections;
using UnityEngine;

public enum EGenerationState
{
    EGS_WaitingForStart,
    EGS_GeneratingTileBlock,
    EGS_SpawningTileBlock,
    EGS_SpawnMileStone,
}
public class TileBlockGenerator : MonoBehaviour
{
    // TODO - State : Calculate all the Future Block to Spawn 
    // TODO - State : Spawn the X next block In the List 
    // TODO - State : Milestone Spawn
    // TODO - Calcul Weight
    // - X =  WeightCategory / TotalWeightCategory
    // - Y =  WeightTileBlock / TotalWeightTileBlockInCategory 
    // - ChanceOfSpawn = X * Y
    
    // TODO - Calculate Coef Difficulty :
    // - Coef = Mathf.Pow(X, NbrMilestone)
    
    public AnimationCurve _difficultyCurve;
    
    public float _Credits = 0;
    
    private EGenerationState _state = EGenerationState.EGS_WaitingForStart;
    private float _generationIndex = -1;
    private Vector3 _startPos;
    

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    
    
    IEnumerator CalculatingTileBlock()
    {
       // Prevoir tous les Block a Generer en Fonction du score actuel du joueur 
       // Prendre En compte le cout de chaque block pour les generer 
       
    }
}
