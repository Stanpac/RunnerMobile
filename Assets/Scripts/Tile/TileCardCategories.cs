using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "RoadTrip/Tiles/TileCategories")]
public class TileCardCategories : ScriptableObject
{
    [SerializeField]
    private Category[] _categories = Array.Empty<Category>();

    [ReadOnly, SerializeField]
    private float _globalWeight; 
    
    public void Clear() => _categories = Array.Empty<Category>();
    
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
    
    // Security for the weight in Editor 
    public void OnValidate()
    {
        float Weight = 0.0f;
        for (int i = 0; i < _categories.Length; ++i) {
            Category category = _categories[i];
            if (category.weight <= 0.0)
                Debug.LogErrorFormat("'{0}' in '{1}' has no weight!", category.name, this);
            for (int j = 0; j < category.cards.Length; ++j) {
                TileCard card = category.cards[j];
                if (card.Weight <= 0.0)
                    Debug.LogErrorFormat("'{0}' in '{1}' has no weight!", card.Prefab.name, this);
                
                
            }
            Weight += category.weight;
        }

        _globalWeight = Weight;
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
