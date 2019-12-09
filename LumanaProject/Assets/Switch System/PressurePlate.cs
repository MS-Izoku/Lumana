using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
[RequireComponent(typeof(Animator))]
public class PressurePlate : SwitchObject {
    [SerializeField] private Animator aniController;
    
    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            ForceState (true);
        }
    }

    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player") {
            ForceState (false);
        }
    }
}