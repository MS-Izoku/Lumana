using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReflectorNode))]
[RequireComponent(typeof(Rigidbody))]
public class NodeGoal : MonoBehaviour
{
    public bool nodeGoalActive = false;
    public bool permanentActive = false;
    public bool onTimer = false;
    public float deactivationTime = 2f;

    public void Activate(){
        Debug.Log("Activated Node Goal: " + gameObject.name);
        nodeGoalActive = true;
        if(!permanentActive)
            if(onTimer)
                StartCoroutine(WaitToDeactivate());
            else
                Debug.Log("Needs a conditional Here");
    }

    private IEnumerator WaitToDeactivate(){
        Debug.Log("Deactivating in -" + deactivationTime + "- seconds");
        yield return new WaitForSeconds(deactivationTime);
        nodeGoalActive = false;
        Debug.Log("Deactivated");
    }
}
