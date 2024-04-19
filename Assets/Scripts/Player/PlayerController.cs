using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/*------------------- Struct / enum -------------------*/
/*------------------- End Struct / enum -------------------*/
/*
 * This script is responsible for managing the player.
 */
public class PlayerController : MonoBehaviour
{
    /*------------------- public / SerializeField variable -------------------*/
    
    public Transform CarModel;
    /*------------------- End public / SerializeField variable -------------------*/
    /*------------------- Private Variables -------------------*/
    
    /*------------------- End Private Variables -------------------*/
    
    public void SetRotation(Quaternion rotation)
    {
        CarModel.rotation = Quaternion.Lerp(CarModel.rotation, rotation, Time.deltaTime * 10f);
    }
}
