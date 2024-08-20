
using System;
using UnityEngine;

/// <summary>
///  store all the luggage data
/// </summary>
public class Luggagelibrary
{
    [SerializeField]
    private luggageData[] _luggages;
    
    public Luggagelibrary() 
    {
        _luggages = new luggageData[0];
    }
    
    public Luggagelibrary(luggageData[] luggages) 
    {
        _luggages = luggages;
    }
    
    public Luggagelibrary(Luggage[] luggages) 
    {
        GenerateLibrary(luggages);
    }
    
    private void GenerateLibrary(Luggage[] luggages)
    {
        _luggages = new luggageData[luggages.Length];
        for (int i = 0; i < luggages.Length; ++i) {
            _luggages[i] = new luggageData(luggages[i], 0);
        }
    }
    
    // get all luggage with minimum 1 count in the library
    public luggageData[] GetLuggages() 
    {
        luggageData[] luggages = new luggageData[0];
        for (int i = 0; i < _luggages.Length; ++i) {
            if (_luggages[i].Count > 0) {
                Array.Resize(ref luggages, luggages.Length + 1);
                luggages[luggages.Length - 1] = _luggages[i];
            }
        }
        return luggages;
    }
    
    // get array of luggages with minimum 1 count in the library, order by stability (low to high)
    public luggageData[] GetLuggagesSortByStability() 
    {
        luggageData[] luggages = GetLuggages();
        Array.Sort(luggages, (a, b) => a.Luggage.Stability.CompareTo(b.Luggage.Stability));
        return luggages;
    }
    
    // get lowest stability luggage in the library
    public luggageData GetLowestStabilityLuggage() 
    {
        luggageData[] luggages = GetLuggagesSortByStability();
        return luggages[0];
    }
    
    // add a luggage to the library
    public void AddLuggage(Luggage luggage, int count) 
    {
        // check if the luggage is already in the library
        for (int i = 0; i < _luggages.Length; ++i) {
            if (_luggages[i].Luggage == luggage) {
                _luggages[i].Count += count;
                return;
            }
        }
        
        // if not, create a new luggage data and insert in the last position of the array
        luggageData luggageData = new luggageData(luggage, count);
        Array.Resize(ref _luggages, _luggages.Length + 1);
        _luggages[_luggages.Length - 1] = luggageData;
    }
    
    // add a array of luggages to the library
    public void AddLuggages(Luggage[] luggages, int count = 1)
    {
        for (int i = 0; i < luggages.Length; ++i) {
            AddLuggage(luggages[i], count);
        }
    }
    
    // remove a specific luggage from the library
    public void RemoveLuggage(Luggage luggage, int count = 1)
    {
        for (int i = 0; i < _luggages.Length; ++i) {
            if (_luggages[i].Luggage == luggage) {
                _luggages[i].Count -= count;
                return;
            }
        }
    }
    
    // remove luggages from the library (lowest to highest stability)
    public void RemoveLowestStabilityLuggages(int count)
    {
        luggageData[] luggages = GetLuggagesSortByStability();
        
        if (count > luggages.Length) {
            count = luggages.Length;
            Debug.LogWarning("trying to remove more luggages than there is in the library");
        }
        
        for (int i = 0; i < count; ++i) {
            RemoveLuggage(luggages[i].Luggage);
        }
    }
    
    // remove all luggage of a specific category 
    public void RemoveLuggageIn(String categoryName)
    {
        Luggage[] luggages = GameManager.Instance.LuggageCategories.GetAllLuggagesInCategory(categoryName); 
        for (int i = 0; i < luggages.Length; ++i) {
            RemoveLuggage(luggages[i]);
        }
    }
    
    // remove luggage of a specific category (low stability in first)
    public void RemoveLowestStabilityLuggageIn(String categoryName, int count = 1)
    {
        Luggage[] luggages = GameManager.Instance.LuggageCategories.GetAllLuggagesInCategory(categoryName); 
        Luggage.SortByStability(ref luggages);
        
        if (count > luggages.Length) {
            count = luggages.Length;
            Debug.LogWarning("trying to remove more luggages than there is in the library");
        }
        
        for (int i = 0; i < count; ++i) {
            RemoveLuggage(luggages[i]);
        }
    }
    
    // remove Random luggages from the library
    public void RemoveRandomLuggage(int count = 1)
    {
        for (int i = 0; i < count; ++i) {
            luggageData[] luggages = GetLuggages();
            int index = UnityEngine.Random.Range(0, luggages.Length);
            RemoveLuggage(luggages[index].Luggage);
        }
    }
    
    // remove all the luggages from the library
    public void RemoveAllLuggages()
    {
        for (int i = 0; i < _luggages.Length; ++i) {
            _luggages[i].Count = 0;
        }
    }
    
    // Get the Weight of the luggage there is in the library
    public float GetTotalWeight()
    {
        float weight = 0;
        for (int i = 0; i < _luggages.Length; ++i) {
            weight += _luggages[i].Luggage.Weight * _luggages[i].Count;
        }
        return weight;
    }
    
    // get total number of luggages in the library
    public int GetTotalLuggageCount()
    {
        int count = 0;
        for (int i = 0; i < _luggages.Length; ++i) {
            count += _luggages[i].Count;
        }
        return count;
    }
    
    /// <summary>
    ///  data of the luggage for the luggage library
    /// </summary>
    [Serializable]
    public struct luggageData
    {
        private Luggage _luggage;
        private int _count;
        private bool _discovered;
        
        public bool Discovered => _discovered;
        public Luggage Luggage => _luggage;
        
        
        public int Count 
        {
            get => _count;
            set
            {
                if (value > 0 && !_discovered) {
                    _discovered = true;
                }
                
                _count = value < 0 ? 0 : value;
            }
        }
        
        public luggageData(Luggage luggage, int count)
        {
            _luggage = luggage;
            _count = count;
            _discovered = count > 0;
        }
    }
}
