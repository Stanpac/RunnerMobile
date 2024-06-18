using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;



public static class ProjectUtility 
{
    [MenuItem("Assets/Create/ProjectUtility/TilePrefab %t", priority = 1)]
    private static void CreatTilePrefab()
    {
        // TODO : Change the Name (+1) si il y en a deja un du meme nom 
        string prefabName = "TilePrefab";
        var prefab = UnityEngine.Resources.Load("Tiles/" + prefabName);
        string targetPath = $"{CurrentProjectFolderPath}/{prefabName}.prefab";
        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(prefab), targetPath);
    }
    
    public static string CurrentProjectFolderPath
    {
        get
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
    }
    
    [MenuItem("GameObject/RoadTrip/TriggerBase", false, priority = 0)]
    private static void CreateTriggerBase(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("TriggerBase");
        go.AddComponent<BoxCollider>();
        go.AddComponent<TriggerBase>();
        
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
       
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}