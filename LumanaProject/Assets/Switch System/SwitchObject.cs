using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObject : MonoBehaviour
{
    protected bool _switchActive = false;
    public virtual bool switchActive{
        get{
            return _switchActive;
        }
        private set{
            this._switchActive = value;
        }
    }

    public bool ToggleSwitch(){
        _switchActive = !_switchActive;
        return _switchActive;
    }

    public bool SetSwitchState(bool activeState){
        _switchActive = activeState;
        return _switchActive;
    }

}
