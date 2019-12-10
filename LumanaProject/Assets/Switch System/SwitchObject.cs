using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObject : MonoBehaviour
{
    [SerializeField]protected bool _switchActive = false; // this only works on startup in the editor
    public virtual bool switchActive{
        get{
            return _switchActive;
        }
        set{
            this._switchActive = value;
        }
    }

    public void ToggleSwitch(){
        _switchActive = !_switchActive;
    }

    public void SetSwitchState(bool activeState){
        _switchActive = activeState;
    }

    public void ForceSwitchObjectState(bool boolState){
        _switchActive = boolState;
    }
}
