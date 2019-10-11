using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReflectorNode))]
[RequireComponent(typeof(Rigidbody))]
public class NodeGoal : MonoBehaviour
{
    public bool nodeGoalActive = false;
    public bool onTimer = false;
    public float deactivationTime = 2f;

    private void OnTriggereEnter(Collider col){
        Debug.Log("Collision");
        if(col.gameObject.tag == "LumanaLight"){
            Debug.Log("Active");
            nodeGoalActive = true;
            if(onTimer)
                StartCoroutine(WaitToDeactivate());
        }
    }

    private IEnumerator WaitToDeactivate(){
        yield return new WaitForSeconds(deactivationTime);
        nodeGoalActive = false;
        Debug.Log("Deactivated");
    }
}
