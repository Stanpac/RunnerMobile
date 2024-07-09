using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct Category
{
    [Tooltip("A name to help identify this category")]
    public string name;
    public TileCard[] cards;
    public float weight;
}

[CreateAssetMenu(menuName = "RoadTrip/Tile/TileCategories")]
public class TileCardCategories : ScriptableObject
{
    public Category[] _categories = Array.Empty<Category>();
    
    public void Clear() => this._categories = Array.Empty<Category>();
    
    public float GetAllWeightsInCategory(Category category)
    {
        float num = 0.0f;
        for (int i = 0; i < category.cards.Length; ++i) {
            num += category.cards[i]._weight;
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
                   
                    float weight = card._weight * num2;
                    weightedSelection.AddChoice(card, weight);
                }
            }
        }
        return weightedSelection;
    }
    
    // Security for the weight in Editor 
    public void OnValidate()
    {
        for (int i = 0; i < _categories.Length; ++i) {
            Category category = this._categories[i];
            if (category.weight <= 0.0)
                Debug.LogErrorFormat("'{0}' in '{1}' has no weight!", category.name, this);
            for (int j = 0; j < category.cards.Length; ++j) {
                TileCard card = category.cards[j];
                if (card._weight <= 0.0)
                    Debug.LogErrorFormat("'{0}' in '{1}' has no weight!", card._tilePrefab.name, this);
            }
        }
    }
}
