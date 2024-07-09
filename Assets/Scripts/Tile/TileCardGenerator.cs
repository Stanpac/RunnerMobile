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
    public GameObject _currentSpawnTarget;
    private int _currentTileCardCost;
    public TileCard LastAttemptedTileCard { get; set; }
    
    private WeightedSelection<TileCard> _tileCardsSelection;
    
    private WeightedSelection<TileCard> FinalTileCardsSelection
    {
        get
        {
            WeightedSelection<TileCard> monsterCardsSelection = _tileCardsSelection;
            if (monsterCardsSelection != null)
                return monsterCardsSelection;
            
            // TODO : Change return type can't be null
            return null;
        }
    }

    private TileCardCategories TileCards
    {
        get => _tileCards;
        set
        {
            if (!(_tileCards != value))
                return;
            _tileCards = value;
            _tileCardsSelection = _tileCards?.GenerateTileCardWeightedSelection();
        }
    }
    
}
