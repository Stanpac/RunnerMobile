using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "RoadTrip/Luggages/LuggageCategories")]
public class LuggageCategories : ScriptableObject
{
    [SerializeField]
    private Category[] _categories = Array.Empty<Category>();
   
    public void Clear() => _categories = Array.Empty<Category>();
    
    // get all the categories
    public Category[] GetAllCategories() => _categories;
    
    // get all the luggages of the given category
    public Luggage[] GetAllLuggagesInCategory(String categoryName)
    {
        Category category = Array.Find(_categories, c => c.name == categoryName);
        return category.luggages;
    }
    
    // get all the luggages
    public Luggage[] GetAllLuggages()
    {
        Luggage[] luggages = new Luggage[0];
        foreach (Category category in _categories) {
            Array.Resize(ref luggages, luggages.Length + category.luggages.Length);
            Array.Copy(category.luggages, 0, luggages, luggages.Length - category.luggages.Length, category.luggages.Length);
        }
        return luggages;
    }
    
    // get a array of luggages (count = size of the Array)
    public Luggage[] PickLuggages(int count)
    {
        Luggage[] luggages = new Luggage[count];
        for (int i = 0; i < count; ++i) {
            Category category = _categories[UnityEngine.Random.Range(0, _categories.Length)];
            Luggage luggage = category.luggages[UnityEngine.Random.Range(0, category.luggages.Length)];
            luggages[i] = luggage;
        }
        return luggages;
    }
    
    // get a array of luggages (count = size of the Array) of the given category 
    public Luggage[] PickLuggages(int count, String categoryName)
    {
        Category category = Array.Find(_categories, c => c.name == categoryName);
        if (category.luggages.Length == 0) {
            Debug.LogWarning("No luggages in the category " + categoryName);
            return Array.Empty<Luggage>();
        }
        
        Luggage[] luggages = new Luggage[count];
        for (int i = 0; i < count; ++i) {
            Luggage luggage = category.luggages[UnityEngine.Random.Range(0, category.luggages.Length)];
            luggages[i] = luggage;
        }
        return luggages;
    }
    
    // get a array of luggages (count = size of the Array) of random category
    public Luggage[] PickLuggagesInRandomCategory(int count)
    {
        Category category = _categories[UnityEngine.Random.Range(0, _categories.Length)];
        if (category.luggages.Length == 0) {
            Debug.LogWarning("No luggages in the category " + category.name);
            return Array.Empty<Luggage>();
        }
        
        Luggage[] luggages = new Luggage[count];
        for (int i = 0; i < count; ++i) {
            Luggage luggage = category.luggages[UnityEngine.Random.Range(0, category.luggages.Length)];
            luggages[i] = luggage;
        }
        return luggages;
    }
    
    // get a array of luggages (count = size of the Array) of category with the given name
    public Luggage[] PickLuggagesInCategory(int count, String categoryName)
    {
        foreach (Category c in _categories) {
            if (c.name == categoryName) {
                if (c.luggages.Length == 0) {
                    Debug.LogWarning("No luggages in the category " + categoryName);
                    return Array.Empty<Luggage>();
                }
        
                Luggage[] luggages = new Luggage[count];
                for (int i = 0; i < count; ++i) {
                    Luggage luggage = c.luggages[UnityEngine.Random.Range(0, c.luggages.Length)];
                    luggages[i] = luggage;
                }
                return luggages;
            }
        }
        Debug.LogErrorFormat("No category with the name " + categoryName);
        return PickLuggagesInRandomCategory(count);
    }
    
    [Serializable]
    public struct Category
    {
        [Tooltip("A name to help identify this category")]
        public string name;
        
        [Tooltip("The icon of this category")]
        public Texture2D icon;
        
        [Tooltip("The luggage in this category")]
        public Luggage[] luggages;
        
        // TODO have the associated music of this category
    }
}
