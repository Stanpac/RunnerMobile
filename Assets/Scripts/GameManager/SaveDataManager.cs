using ScriptableObjects;
using UnityEditor;
using UnityEngine;


/*
 * This script is responsible for managing the save data.
 */
public class SaveDataManager : MonoBehaviour
{
    public SaveData _CurrentSaveData;
    
    public void Save()
    {
        // Save the data
    }

    public void Load()
    {
        //_CurrentSaveData = AssetDatabase.LoadAssetAtPath<SaveData>("Assets/ScriptableObject/SaveData/Start/StartData.asset");
    }
}
