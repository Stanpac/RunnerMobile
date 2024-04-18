using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float Speed;
    public float Distance;

    Vector3 dir = new Vector3(0, 0, -1);

    private void Start()
    {
        float i = Random.Range(0, 1);
        if(i > .5f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    void Update()
    {
        
        transform.Translate(dir * Speed * Time.deltaTime);

        if (transform.position.z < -Distance)
        {
            Destroy(gameObject);
        }
    }
}