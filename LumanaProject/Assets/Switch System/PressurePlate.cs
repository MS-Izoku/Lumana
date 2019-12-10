using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
[RequireComponent(typeof(Animator))]
public class PressurePlate : SwitchObject {
    [SerializeField] private Animator aniController;

    public override bool switchActive{
        get{
            return aniController.GetBool("Active");
        }
        set{
            this._switchActive = value;
            aniController.SetBool("Active" , _switchActive);
        }
    }
    void Awake(){
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            this.ForceSwitchObjectState (true);
        }
    }

    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player") {
            this.ForceSwitchObjectState (false);
        }
    }

    private void Update() {
        Debug.Log(switchActive);    
    }
}