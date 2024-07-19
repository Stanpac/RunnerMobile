using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class TileCardGenerator : MonoBehaviour
{
    [SerializeField][BoxGroup("Credits")]
    private float _creditsAvailable;
    [SerializeField][BoxGroup("Credits")]
    private float _initialCredits = 1;
    [SerializeField][BoxGroup("Credits")]
    private bool _skipSpawnIfTooCheap = true;
    [SerializeField][BoxGroup("Credits")]
    private int _maximumNumberMultiplicatorBeforeConsideredCheap = 6;
    private int _consecutiveCheapSkips;
    private int _maxConsecutiveCheapSkips = int.MaxValue;
    
    [SerializeField][BoxGroup("TileCard")]
    private bool _resetTilerCardIfFailed = true;
    
    [SerializeField][BoxGroup("TileCard")]
    private TileCardCategories _tileCards;
    
    [SerializeField, ReadOnly][BoxGroup("TileCard")]
    private TileCard _currentTileCard;
    
    private TileCard LastAttemptedTileCard { get; set; }
    
    private WeightedSelection<TileCard> _tileCardsSelection;
    
    [SerializeField, ReadOnly][BoxGroup("Tiles")]
    private GameObject _previousTileSpawned;
    [SerializeField, ReadOnly][BoxGroup("Tiles")]
    private List<GameObject> _allSpawnedTilesSpawned = new List<GameObject>();

    [SerializeField][BoxGroup("Generation Parameters")]
    private  float _distanceOfGeneration = 100.0f; 
    private float _distanceOfDestruction;
    
    [SerializeField][BoxGroup("SafeCounter")]
    private int _safeCounterMax = 200;
    
    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TileManager = this;
    }
    
    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TileManager = null;
    }
    
    private WeightedSelection<TileCard> FinalTileCardsSelection
    {
        get
        {
            WeightedSelection<TileCard> tileCardsSelection = _tileCardsSelection;
            if (tileCardsSelection != null)
                return tileCardsSelection;
            
            // TODO : Change return type can't be null
            return null;
        }
    }
    
    private void Awake()
    {
        _creditsAvailable = _initialCredits;
        _distanceOfDestruction = _distanceOfGeneration * 1.1f;
        
        _tileCardsSelection = _tileCards?.GenerateTileCardWeightedSelectionAffordable(_creditsAvailable);
    }
    
    private void Start()
    {
        int safeCounter = 0;
        while ((_previousTileSpawned == null || 
               _previousTileSpawned.transform.position.z < _distanceOfGeneration)
               && safeCounter <_safeCounterMax)
        {
            PrepareNewTileCard(_tileCardsSelection.Evaluate());
            GenerateTileCard();
            safeCounter++;
        }
    }
    
    
    private void FixedUpdate()
    {
        int safeCounter = 0;
        while ((_previousTileSpawned == null || 
               _previousTileSpawned.transform.position.z < _distanceOfGeneration) 
               && safeCounter <_safeCounterMax)
        {
            GenerateTileCard();
            safeCounter++;
        }
        
        if (_allSpawnedTilesSpawned.Count <= 0) return;
        
        if (_allSpawnedTilesSpawned.Count > 0 && _allSpawnedTilesSpawned[0].transform.position.z < _distanceOfDestruction) {
           Destroy(_allSpawnedTilesSpawned[0].gameObject);
           _allSpawnedTilesSpawned.RemoveAt(0);
        }
    }
    
    
    public void GenerateTileCard()
    {
        Transform targetTransform = transform;
        if (_previousTileSpawned == null) {;
            targetTransform.position = Vector3.zero;
            targetTransform.rotation = Quaternion.identity;
        } else {
            targetTransform.position = _previousTileSpawned.transform.position;
            targetTransform.rotation = Quaternion.identity; 
        }
        
        if (!AttemptSpawnOnTarget(targetTransform)) {
            if (_resetTilerCardIfFailed) {
               _currentTileCard = null;
            }
        }
    }
    
    private void PrepareNewTileCard(TileCard overrideTileCard)
    {
        Debug.LogFormat("Preparing Tile Card {0}", overrideTileCard.RoadTilePrefab);
        _currentTileCard = overrideTileCard;
        LastAttemptedTileCard = _currentTileCard;
    }
    
    // Get The TileCard the most Expensive to spawn in The TileCardSelection
    private int MostExpensiveTileCostInDeck {
        
        get
        {
            var a = 0;
            for (var i = 0; i < FinalTileCardsSelection.Count; ++i) {
                var tileCard = FinalTileCardsSelection.GetChoice(i).value;
                var b = tileCard.CreditsCount;
                a = Mathf.Max(a, b);
            }

            return a;
        }
    }

    private bool AttemptSpawnOnTarget(Transform spawnTarget)
    {
        // TODO : rewrite this function because No 
        if (_currentTileCard == null) {
            Debug.Log("No TileCard Selected, pick new one.");
            if (FinalTileCardsSelection == null) 
                return false;
            PrepareNewTileCard(FinalTileCardsSelection.Evaluate());
        }
        
        if (_creditsAvailable < _currentTileCard.CreditsCount) {
            Debug.LogFormat("Spawn card {0} is too expensive, aborting spawn.", _currentTileCard.RoadTilePrefab);
            return false;
        }

        if (_skipSpawnIfTooCheap 
            && _consecutiveCheapSkips < _maxConsecutiveCheapSkips
            && _currentTileCard.CreditsCount * _maximumNumberMultiplicatorBeforeConsideredCheap < _creditsAvailable) 
        {
            Debug.LogFormat("Card {0} seems too cheap. Comparing against most expensive possible ({1})", 
                _currentTileCard.RoadTilePrefab, MostExpensiveTileCostInDeck);
            
            if (MostExpensiveTileCostInDeck > _currentTileCard.CreditsCount) {
                ++_consecutiveCheapSkips;
                Debug.LogFormat("Card {0} is too cheap, skipping.", _currentTileCard.RoadTilePrefab);
                return false;
            }
        }
        
        var spawnCard = _currentTileCard;
        
        // try to recup the collider of the tile
        Collider collider = spawnCard.RoadTilePrefab.GetComponent<Collider>();
        if (collider == null) {
            collider = spawnCard.RoadTilePrefab.GetComponentInChildren<Collider>();
        }
        
        if (collider == null) {
            Debug.LogError("No Collider found in the TileCard");
            return false;
        }
        
        // Add Offset of the tile to spawn at the SpawnTarget
        Vector3 offsetposition = Vector3.zero;
        offsetposition.z += collider.bounds.extents.z;
        spawnTarget.position += offsetposition;
        
        var spawnTarget1 = spawnTarget;
        
        if (!Spawn(spawnCard, spawnTarget1)) {
            return false;
        }
        _creditsAvailable -= _currentTileCard.CreditsCount;
        _consecutiveCheapSkips = 0;
        AddCreditsAfterSpawn();
        GenerateWeightedSelectionWeCanBuy();
        return true;
    }

    private void GenerateWeightedSelectionWeCanBuy()
    {
        WeightedSelection<TileCard> weightedSelection = _tileCards.GenerateTileCardWeightedSelection();
        _tileCardsSelection = new WeightedSelection<TileCard>();
        for (int i = 0; i < weightedSelection.Count; ++i) {
            TileCard tileCard = weightedSelection.GetChoice(i).value;
            if (tileCard.CreditsCount <= _creditsAvailable) {
                _tileCardsSelection.AddChoice(tileCard, tileCard.Weight);
            }
        }
    }

    public bool Spawn(TileCard spawnCard, Transform spawnTarget)
    {
        GameObject roadTile = null;
        roadTile = spawnCard.DoSpawn(spawnTarget.position, spawnTarget.rotation).spawnedInstance;
        if (gameObject == null) {
            Debug.LogFormat("Spawn card {0} failed to spawn. Aborting cost procedures.", spawnCard);
            return false;
        }
        
        _previousTileSpawned = roadTile;
        _allSpawnedTilesSpawned.Add(roadTile);
        return true;
    }
    
    private void AddCreditsAfterSpawn()
    {
        Debug.LogFormat("Adding Credits after spawn {0}", GameManager.Instance.scoreManager.GetCoefDifficulty());
        _creditsAvailable += GameManager.Instance.scoreManager.GetCoefDifficulty();
        Debug.LogFormat("Credits Available now : {0}", _creditsAvailable);
    }
}
