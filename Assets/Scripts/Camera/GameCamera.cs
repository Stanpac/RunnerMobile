using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GameCamera : MonoBehaviour
{ 
    public Transform target {get; set;}
    
    [SerializeField, BoxGroup("Offset")] Vector3 moveOffset;
    [SerializeField, BoxGroup("Offset")] Vector3 rotOffset;
    
    void Update()
    {
        LookAtTarget();
    }

    private void LookAtTarget()
    {
        if (target == null) return;
        Vector3 targetPos = target.TransformPoint(moveOffset);
        transform.position = targetPos;
        transform.LookAt(target.position + rotOffset);
    }
    
}
