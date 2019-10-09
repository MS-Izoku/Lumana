using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSensor : MonoBehaviour
{
    [HideInInspector] public MovingPlatform platform;
    [HideInInspector] public Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other){
        rb = other.GetComponent<Rigidbody>();
        if(rb != null)
            platform.AddToPlatform(rb);
    }
    
    void OnTriggerExit(Collider other){
        rb = other.GetComponent<Rigidbody>();
        if(rb != null)
            platform.RemoveFromPlatform(rb);
        
    }
}
