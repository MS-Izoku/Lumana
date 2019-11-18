using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Pickup : MonoBehaviour
{
    public AudioSource audioField;
    public Item item;

    void Start(){
        audioField = GetCompnent<AudioSource>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player")
            PickupItem();
    }

    public virtual void PickupItem(){
        Debug.Log("PickUp");
        AudioSource.Play(0);
        Destroy(this.gameObject);
        // Add To inventory
    }
}