using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGate : SwitchObject
{
    [SerializeField] private List<SwitchObject> switches = new List<SwitchObject>();
    public List<bool> combination = new List<bool>(); // the usage of this will between gate types

    public override bool switchActive{
        get{
            return _switchActive;
        }
        set{
            if(checkActive()) this._switchActive = true;
            else this._switchActive = false;
        }
    }

    private void Start(){
        for(int i = 0; i < switches.Count; i++){

        }
    }

    public virtual bool checkActive(){
        // need to figure out the best solution for n many gate inputs
        // there will be many gates in a level, so this needs some optomization
        Debug.Log("Checking Active:");

        return true;
    }
}
