using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/*
 * This script is responsible for generating the tiles that make up the terrain.
 * It instantiates the tiles in the scene 
 */
public class OldTileGenerator : MonoBehaviour
{
    [SerializeField] Tile[] Tiles;
    [SerializeField] float DistanceStartGeneration;
    
    [SerializeField] int TilesToAvoid = 1;
    [SerializeField] List<int> PreviousIndex;
    [SerializeField] float TerrainSpeed;
    
    private Vector3 Pos;
    private int i;
    
    private Tile PreviousTile;
    private Renderer previousRend;
    private Renderer NextRend;

    /* private void Start()
    {
        i = Random.Range(0, Tiles.Length);
        SpawnTile(new Vector3(0, transform.position.y, DistanceStartGeneration));
    }

    private void Update()
    {
        if (previousRend == null ) return;
        
        float d = PreviousTile.transform.position.z + previousRend.bounds.extents.z - (TerrainSpeed * Time.deltaTime);
        Pos = new Vector3(0, transform.position.y, d);

        if (d < DistanceStartGeneration)
        {
            SpawnTile(Pos);
        }
    }
    
    
    void SpawnTile(Vector3 Point)
    {
        Vector3 pos = Point;
        pos.z += Tiles[i].GetComponentInChildren<Renderer>().bounds.extents.z;

        PreviousTile = Instantiate(Tiles[i], pos, Quaternion.identity, transform);
        PreviousIndex.Add(i);

        PreviousTile.GetComponent<Tile>().Speed = TerrainSpeed;
        PreviousTile.GetComponent<Tile>().Distance = DistanceStartGeneration;
        previousRend = PreviousTile.GetComponentInChildren<Renderer>();

        if (PreviousIndex.Count > TilesToAvoid)
        {
            PreviousIndex.RemoveAt(0);
        }
        
        if (Tiles.Length > 1)
        {
            while (PreviousIndex.Contains(i))
            {
                i = Random.Range(0, Tiles.Length);
            }
        }
        else
            i = Random.Range(0, Tiles.Length);

        NextRend = Tiles[i].GetComponentInChildren<Renderer>();

    }
    
    private void OnDrawGizmos()
    {
        if (NextRend == null) return;
                
        Vector3 pos = Pos;
        pos.z += NextRend.bounds.extents.z;
        
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(Pos, .2f);
        Gizmos.DrawWireCube(pos, NextRend.bounds.size);
        Gizmos.color = UnityEngine.Color.green;
        pos = new Vector3(0, 0, DistanceStartGeneration + NextRend.bounds.extents.z);
        Gizmos.DrawWireCube(pos, NextRend.bounds.size);
    }*/
}
