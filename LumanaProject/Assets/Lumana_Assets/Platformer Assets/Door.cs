using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public NodeGoal goal;
    public bool isOpen;
    public Animator animator;

    void Start(){
        StartCoroutine(OpenDoor());
        animator = GetComponent<Animator>();
    }

    private IEnumerator OpenDoor(){
        while(!goal.nodeGoalActive){ Debug.Log("Locked"); yield return null; }

        Debug.Log("Opening Door");
        if(!goal.permanentActive){
            // while(goal.nodeGoalActive){
            //     if(!goal.nodeGoalActive) break;
            //     Debug.Log("Waiting");
            // }
            Debug.Log("Closing");
            // close door here
        }

        // open the door with the animator
        yield return null;

        Debug.Log("Door has been opened");
        yield return null;
    }
}
