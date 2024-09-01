using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;



public static class ProjectEditorUtility 
{
    public static readonly string TILEPREFABNAME = "BaseTilePrefab";
    public static readonly string TILEPREFABPATH = "Tiles/BaseTilePrefab";
    
    [MenuItem("Assets/Create/RoadTrip/Tiles/TilePrefab %t", priority = 1)]
    private static void CreatTilePrefab()
    {
        var prefab = UnityEngine.Resources.Load(TILEPREFABPATH);
        GameObject prefabRef = (GameObject)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(prefab));
        GameObject instanceRoot = (GameObject)PrefabUtility.InstantiatePrefab(prefabRef);
        string targetPath = $"{CurrentProjectFolderPath}/{"New Tile Prefab"}.prefab";
        targetPath = AssetDatabase.GenerateUniqueAssetPath(targetPath);
        GameObject pVariant = PrefabUtility.SaveAsPrefabAsset(instanceRoot, targetPath);
        GameObject.DestroyImmediate(instanceRoot);
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
}