using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


/*
 * This script is responsible for managing the save data.
 */
public class SaveDataManager : MonoBehaviour
{
    [FormerlySerializedAs("_CurrentSaveData")] public SO_Save currentSoSave;
    
    public void Save()
    {
        // Save the data
    }

    public void Load()
    {
        //_CurrentSaveData = AssetDatabase.LoadAssetAtPath<SaveData>("Assets/ScriptableObject/SaveData/Start/StartData.asset");
    }
}
