using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class NodeGoal : MonoBehaviour
{
    public bool nodeGoalActive = false;
    public bool onTimer = false;
    public float deactivationTime = 2f;

    private void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "LumanaLight"){
            nodeGoalActive = true;
            if(onTimer)
                StartCoroutine(WaitToDeactivate());
        }
    }

    private IEnumerator WaitToDeactivate(){
        yield return new WaitForSeconds(deactivationTime);
        nodeGoalActive = false;
    }
}
