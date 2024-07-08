using UnityEngine;


public class TileBlock : ScriptableObject 
{
    [SerializeField] private Collider _boundsCollider;
    public Bounds Bounds => _boundsCollider.bounds;
    // TODO - Variation dans les Pieges sur la tile ?
}
