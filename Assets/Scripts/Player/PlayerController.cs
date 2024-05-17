using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

// This script is responsible for managing the player.
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Transform CarModel;
    
    public void SetRotation(Quaternion rotation)
    {
        CarModel.rotation = Quaternion.Lerp(CarModel.rotation, rotation, Time.deltaTime * 10f);
    }
}
