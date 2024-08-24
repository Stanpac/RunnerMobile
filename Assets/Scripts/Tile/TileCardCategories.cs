using System;
using System.Diagnostics.CodeAnalysis;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "RoadTrip/Tiles/TileCategories")]
public class TileCardCategories : ScriptableObject
{
    [SerializeField]
    [ValidateInput("HaveOneCards", "You must Have at least one MileStone Card")]
    private TileCard[] _milesStonesCards = Array.Empty<TileCard>();
    
    [SerializeField]
    [ValidateInput("CategoryValidation", "Cateories have a problem, please check that that the category has a name and that cards in categories have a weight > 0 and a credit count > 0")]
    private Category[] _categories = Array.Empty<Category>();

    [ReadOnly, SerializeField]
    private float _globalWeight; 
    
    public void Clear() => _categories = Array.Empty<Category>();

    public bool HasMilestoneCard()
    {
        return _milesStonesCards.Length > 0;
    }
    public float GetAllWeightsInCategory(TileCardCategories.Category category)
    {
        float num = 0.0f;
        for (int i = 0; i < category.cards.Length; ++i) {
            num += category.cards[i].Weight;
        }
        return num;
    }
    
    public WeightedSelection<TileCard> GenerateTileCardWeightedSelection()
    {
        WeightedSelection<TileCard> weightedSelection = new WeightedSelection<TileCard>();
        
        for (int i = 0; i < _categories.Length; ++i) {
            float num1 = GetAllWeightsInCategory(_categories[i]);
            float num2 = _categories[i].weight / num1;
            if ( num1 > 0.0) {
                foreach (TileCard card in _categories[i].cards)  {
                   
                    float weight = card.Weight * num2;
                    weightedSelection.AddChoice(card, weight);
                }
            }
        }
        return weightedSelection;
    }
    
    public WeightedSelection<TileCard> GenerateTileCardWeightedSelectionAffordable(float creditsAvailable)
    {
        WeightedSelection<TileCard> weightedSelection = new WeightedSelection<TileCard>();
        
        for (int i = 0; i < _categories.Length; ++i) {
            float num1 = GetAllWeightsInCategory(_categories[i]);
            float num2 = _categories[i].weight / num1;
            if ( num1 > 0.0) {
                foreach (TileCard card in _categories[i].cards)  {
                    if (card.CreditsCount <= creditsAvailable ) {
                        // need to check if the card is Too Cheep to be considered ?
                        float weight = card.Weight * num2;
                        weightedSelection.AddChoice(card, weight);
                    }
                }
            }
        }
        return weightedSelection;
    }
    
    public TileCard GetRandomMilestoneCard()
    {
        return _milesStonesCards?[UnityEngine.Random.Range(0, _milesStonesCards.Length)];
    }
    
    // Security for the weight in Editor 
    public void OnValidate()
    {
        float weight = 0.0f;
        for (int i = 0; i < _categories.Length; ++i) {
            weight += _categories[i].weight;
        }
        _globalWeight = weight;
    }
    
    private bool HaveOneCards(TileCard[] cards)
    {
        return cards.Length > 0;
    }
    
    private bool CategoryValidation(Category[] categories)
    {
        for (int i = 0; i < _categories.Length; ++i) {
            Category category = _categories[i];
            if (category.name == "" || category.weight <= 0.0 || category.cards.Length <= 0)
                 return false;
            for (int j = 0; j < category.cards.Length; ++j) {
                TileCard card = category.cards[j];
                if (card.Weight <= 0.0)
                    return false;
            }
        }
        
        if (_categories.Length > 0) {
            foreach (var category in _categories) {
                if (category.cards.Length <= 0) {
                    return false;
                }

                foreach (var tileCard in category.cards) {
                    if (tileCard.Weight <= 0.0f || tileCard.CreditsCount <= 0.0f) {
                        return false;
                    }
                }
            }   
        }
        return true;
    }
    
    [Serializable]
    public struct Category
    {
        [Tooltip("A name to help identify this category")]
        public string name;
    
        [Tooltip("The weight of this category in the selection process")]
        public float weight;
        
        [Tooltip("The cards in this category")]
        public TileCard[] cards;
    }
}
