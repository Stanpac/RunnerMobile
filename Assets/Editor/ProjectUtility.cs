using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;



public static class ProjectUtility 
{
    [MenuItem("Assets/Create/ProjectUtility/TilePrefab", priority = 1)]
    private static void CreatTilePrefab()
    {
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
}