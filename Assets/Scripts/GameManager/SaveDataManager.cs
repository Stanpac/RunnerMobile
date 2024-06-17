using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;



public class SaveDataManager : MonoBehaviour
{
    public SO_Save _currentSoSave;
    
    public void Save()
    {
        // Save the data
    }

    public void Load()
    {
        //_CurrentSaveData = AssetDatabase.LoadAssetAtPath<SaveData>("Assets/ScriptableObject/SaveData/Start/StartData.asset");
    }
}
