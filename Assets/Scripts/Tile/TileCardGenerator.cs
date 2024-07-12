using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class TileCardGenerator : MonoBehaviour
{
    public float _creditsAvailable;
    
    public bool _skipSpawnIfTooCheap = true;
    private int _consecutiveCheapSkips;
    public int _maxConsecutiveCheapSkips = int.MaxValue;
    
    public bool _resetTilerCardIfFailed = true;
    
    public TileCardCategories _tileCards;
    
    public TileCard _currentTileCard;
    
    public TileCard LastAttemptedTileCard { get; set; }
    
    private WeightedSelection<TileCard> _tileCardsSelection;
    
    private GameObject _previousTileSpawned;
    private List<GameObject> _allSpawnedTilesSpawned = new List<GameObject>();

    public  float _distanceOfGeneration = 100.0f; 
    private float _distanceOfDestruction;
    
    public int _safeCounterMax = 200;
    
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
        // TODO Initial Credits ?
        _creditsAvailable = 1;
        _distanceOfDestruction = _distanceOfGeneration * 1.1f;
        
        // Generate the selection of tile cards
        _tileCardsSelection = _tileCards?.GenerateTileCardWeightedSelection();
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
        Debug.LogFormat("Preparing Tile Card {0}", overrideTileCard._roadTilePrefab);
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
                var b = tileCard._creditsCount;
                a = Mathf.Max(a, b);
            }

            return a;
        }
    }

    private bool AttemptSpawnOnTarget(Transform spawnTarget)
    {
        if (_currentTileCard == null) {
            Debug.Log("No TileCard Selected, pick new one.");
            if (FinalTileCardsSelection == null) 
                return false;
            PrepareNewTileCard(FinalTileCardsSelection.Evaluate());
        }
        
        if (_creditsAvailable <= _currentTileCard._creditsCount) {
            Debug.LogFormat("Spawn card {0} is too expensive, aborting spawn.", _currentTileCard._roadTilePrefab);
            return false;
        }

        if (_skipSpawnIfTooCheap && _consecutiveCheapSkips < _maxConsecutiveCheapSkips) {
            Debug.LogFormat("Card {0} seems too cheap. Comparing against most expensive possible ({3})",
                _currentTileCard._roadTilePrefab, MostExpensiveTileCostInDeck);
            
            if (MostExpensiveTileCostInDeck > _currentTileCard._creditsCount) {
                ++_consecutiveCheapSkips;
                Debug.LogFormat("Card {0} is too cheap, skipping.", _currentTileCard._roadTilePrefab);
                return false;
            }
        }
        
        var spawnCard = _currentTileCard;
        
        // try to recup the collider of the tile
        Collider collider = spawnCard._roadTilePrefab.GetComponent<Collider>();
        if (collider == null) {
            collider = spawnCard._roadTilePrefab.GetComponentInChildren<Collider>();
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
        _creditsAvailable -= _currentTileCard._creditsCount;
        _consecutiveCheapSkips = 0;
        AddCreditsAfterSpawn();
        return true;
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
        _creditsAvailable += GameManager.Instance.scoreManager.GetCoefDifficulty();
    }
}
