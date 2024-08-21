using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Shape 
{
    Square,
    Circle,
    Custom,
}

public class AssetGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("ShapeOfGeneratorZone")]
    public Shape shape = Shape.Square;

    [Header("Parametres")]
    public bool Bo_GenerateOnStart = false;
    [Space(10)]
    public int I_Iterations = 5;
    public float Ft_RayLenght = 100;
    public float I_SpawnHeight = 2;
    public LayerMask Lm_Mask;

    [Space(25)]
    public Vector3 V3_BoundA = new Vector3(10, 10, 10);
    public Vector3 V3_BoundB = new Vector3(-10, 10, -10);
    [Space(25)]

    public KeyCode Key_Generate;
    public KeyCode Key_Clear;
    [Space(25)]

    public GameObject[] Prefabs;
    [HideInInspector] public List<GameObject> spawnedObjects = new List<GameObject>();


    private void Start()
    {
        if (Bo_GenerateOnStart)
        {
            V_ClearObjects();
            V_GenerateObjects();
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(Key_Generate))
        {
            V_ClearObjects();
            V_GenerateObjects();
        }

        if (Input.GetKeyDown(Key_Clear))
        {
            V_ClearObjects();
        }

    }

    public void V_GenerateObjects()
    {
        //create empty
        GameObject Parent = new GameObject(this.gameObject.name + "Generated Objects");

        if (shape == Shape.Square)
        {
            for (int i = 0; i < I_Iterations; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(V3_BoundA.x, V3_BoundB.x), Random.Range(V3_BoundA.y, V3_BoundB.y), Random.Range(V3_BoundA.z, V3_BoundB.z));
                GenerateRay(randomPos, Parent);
            }
        }
        else if (shape == Shape.Circle)
        {
            for (int i = 0; i < I_Iterations; i++)
            {
                //find a random point in a circle with V3_BoundA as the center and V3_BoundB a point on the edge
                Vector3 randomPos = V3_BoundA + Random.insideUnitSphere * Vector3.Distance(V3_BoundA, V3_BoundB);
                randomPos.y = V3_BoundA.y;
                
                GenerateRay(randomPos, Parent);
            }

        }
        else if (shape == Shape.Custom)
        {
             Debug.LogError("Custom shape not implemented");
        }
        else
        {
            Debug.LogError("Shape not found");
        }
    }

    public void V_ClearObjects()
    {
        if (spawnedObjects.Count <= 0) return;

        foreach (var item in spawnedObjects)
        {
            DestroyImmediate(item.gameObject);
        }

        spawnedObjects.Clear();
        DestroyImmediate(GameObject.Find(this.gameObject.name + "Generated Objects"));
    }

    void GenerateRay(Vector3 pos, GameObject Parent)
    {

        Debug.DrawRay(pos, Vector3.down * Ft_RayLenght, Color.green, 0.05f);
        RaycastHit hit;

        bool ground = Physics.Raycast(pos, Vector3.down, out hit, Ft_RayLenght, Lm_Mask);

        if (ground)
        {
            InstantiateObject(hit.point, Parent);
        }
    }

    void InstantiateObject(Vector3 touch, GameObject Parent)
    {
        if (Prefabs.Length <= 0) return;

        int r = (int)Random.Range(0, Prefabs.Length);
        Quaternion q = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GameObject TempObj= PrefabUtility.InstantiatePrefab(Prefabs[r], Parent.transform) as GameObject;
        TempObj.transform.position = touch + new Vector3(0, I_SpawnHeight, 0);
        TempObj.transform.rotation = q;
        spawnedObjects.Add(TempObj);
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(AssetGenerator))]
public class RocheGeneratorEditor : Editor
{
    public void OnSceneGUI()
    {

        var t = target as AssetGenerator;
        var A = t.V3_BoundA;
        var B = t.V3_BoundB;
        var R = t.Ft_RayLenght;

        if (t.shape == Shape.Square)
        {
            Handles.color = Color.white;

            var C = new Vector3(A.x, A.y, B.z);
            var D = new Vector3(B.x, B.y, A.z);

            Handles.DrawSolidDisc(A, Vector3.up, 0.6f);
            Handles.DrawSolidDisc(B, Vector3.up, 0.6f);

            Handles.color = Color.yellow;

            Handles.DrawLine(A, C);
            Handles.DrawLine(C, B);
            Handles.DrawLine(B, D);
            Handles.DrawLine(D, A);

            Handles.DrawDottedLine(A, B, 4);

            Handles.color = Color.grey;
            Handles.DrawDottedLine(A, A + Vector3.down * R, 2);
            Handles.DrawDottedLine(B, B + Vector3.down * R, 2);

        }
        else if (t.shape == Shape.Circle)
        {
            Handles.color = Color.white;
            Handles.DrawSolidDisc(A, Vector3.up, 0.6f);

            Handles.color = Color.yellow;
            Handles.DrawWireDisc(A, Vector3.up, Vector3.Distance(A,B));

            Handles.color = Color.grey;
            Handles.DrawDottedLine(A, A + Vector3.down * R, 2);
            Handles.DrawDottedLine(B, B + Vector3.down * R, 2);
        }
        else if (t.shape == Shape.Custom) 
        { 
        
        }

        EditorGUI.BeginChangeCheck();

        Vector3 newPosA = Handles.PositionHandle(A, Quaternion.identity);
        Vector3 newPosB = Handles.PositionHandle(B, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(t, "Moved Spawn Area");
            t.V3_BoundA = newPosA;
            t.V3_BoundB = newPosB;
        }
    }

    public override void OnInspectorGUI()
    {
        var t = target as AssetGenerator;

        base.OnInspectorGUI();


        // Boutons "g�n�rer" et "supprimer"

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Generer"))
        {
            t.V_ClearObjects();
            t.V_GenerateObjects();
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Supprimer"))
        {
            t.V_ClearObjects();
        }

        GUILayout.EndHorizontal();

    }
}
#endif