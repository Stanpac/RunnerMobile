using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
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
    
    // Tile Trop Bon marché ? 
    // - Credit X * la value du T1 Du block 
    // - ce n'est pas le + cher qu'il puisse faire spawn 
    
    public AnimationCurve _difficultyCurve;
    
    public float _Credits = 0;
    
    public EGenerationState _state = EGenerationState.EGS_WaitingForStart;
    public float _generationIndex = -1;
    public Vector3 _startPos;
    
    public int MaxTileBlockBeforeSpawningMileStone = 40;
    
    public List<TileCategory> _tileCategories;
    public List<SpawnTileData> _TileDataCalculating;
    
    public int _currentTileDataIndex = 0;
    

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    
    // Start the Calculating of the TileBlock
    IEnumerator CalculatingTileBlock()
    {
       // get list of All Category possible ?
       List<TileCategory> possibleCategory = GetPossibleCategory();
       if (possibleCategory == null) yield break;
       
       // Generate List of TileBlock to Spawn with the list _possibleCategory
       _TileDataCalculating = GenerateTileBlockList(possibleCategory);
       
    }
    
    // Generate a List of TileBlock to Spawn with the List of Possible Category 
    private List<SpawnTileData> GenerateTileBlockList(List<TileCategory> possibleCategory)
    {
        List<SpawnTileData> tileBlockList = new List<SpawnTileData>();
        float safeCounter = 0;
        
        do {
            // Select a Category
            TileCategory selectedCategory = SelectCategory(possibleCategory);
            if (selectedCategory == null) {
                Debug.LogError("No Category Selected");
                continue;
            }
            
            // Select a TileBlock in the Category
            SpawnTileData selectedTileBlock = selectedCategory.GetTileBlock(_Credits);
            if (selectedTileBlock._tileBlock == null) {
                Debug.LogError("No TileBlock Selected");
                continue;
            }
            
            safeCounter++;
            AddCredit();
        } while (tileBlockList.Count < MaxTileBlockBeforeSpawningMileStone && safeCounter < 100);
        
        if (tileBlockList.Count == 0) {
            Debug.LogError("TileBlockList is Empty");
            return null;
        }
        
        return tileBlockList;
    }
    
    // Select a Category in the List of Possible Category (Weighted)
    private TileCategory SelectCategory(List<TileCategory> possibleCategory)
    {
        // Calculate Total Weight
        float totalWeight = 0;
        foreach (TileCategory tileCategory in possibleCategory) {
            totalWeight += tileCategory.GetCategoryWeight();
        }
        
        // Select the random Value
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float currentWeight = 0;
        
        // Select the Category
        foreach (TileCategory tileCategory in possibleCategory) {
            currentWeight += tileCategory.GetCategoryWeight();
            if (currentWeight >= randomValue) {
                return tileCategory;
            }
        }
        
        return null;
    }
    
    // Add Credit For the Next TileBlockGeneration
    private void AddCredit()
    {
        _Credits += 1 * GameManager.Instance.scoreManager.GetCoefDifficulty();
    }
    
    // Return the List of Possible Category ( MileStoneCondition <= MileStoneIndex) 
    private List<TileCategory> GetPossibleCategory()
    {
        List<TileCategory> possibleCategory = new List<TileCategory>();
        foreach (TileCategory tileCategory  in _tileCategories) {
            if (tileCategory.GetMilestonesCondition() > GameManager.Instance.scoreManager.GetMileStoneIndex()) {
                continue;
            }
           
            possibleCategory.Add(tileCategory);
        }
       
        if (possibleCategory.Count == 0) {
            Debug.LogError("No Category Available, you should have at least one category at _milestonesCondition = 0");
            return null;
        }
        
        return possibleCategory;
    }
}
