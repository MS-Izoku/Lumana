using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorNode : MonoBehaviour
{
    public bool isRotating = false;
    public float speed = 5f;
    public float pauseTime = 0.5f;
    public float minAngle = -90;
    public float maxAngle = 90;
    public float yAdjust = 1f;

    private Vector3 adjustedPos;
    private Vector3 minTarget;
    private Vector3 maxTarget;

    public bool goToMin = true;

    private Quaternion minAngleRotation;
    private Quaternion maxAngleRotation;
    private bool gameStart = false;
    
    void Start()
    {
        gameStart = true;
        Setup();
        
        if(goToMin)
            StartCoroutine(RotateNode(minAngleRotation));
        else
            StartCoroutine(RotateNode(maxAngleRotation));
    }

    void OnDrawGizmos(){
        if(!gameStart){
            Setup();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(adjustedPos , minTarget);
            Gizmos.DrawLine(adjustedPos , maxTarget);
        }
    }

    private void Setup(){
        adjustedPos = transform.position + (Vector3.up * yAdjust);
        Quaternion minAngleRotationEuler = Quaternion.Euler(0 , minAngle , 0);
        Quaternion maxAngleRotationEuler = Quaternion.Euler(0 , maxAngle , 0);

        Vector3 target =  transform.rotation * (Vector3.forward * 10f);   // GUI
        minTarget = (minAngleRotation * target) + transform.position;   // GUI
        maxTarget = (maxAngleRotation * target) + transform.position; // GUI

        minAngleRotation = Quaternion.AngleAxis(minAngle , transform.up) * transform.rotation;
        maxAngleRotation = Quaternion.AngleAxis(maxAngle , transform.up) * transform.rotation;
    }

    private IEnumerator RotateNode(Quaternion targetAngle){        
        if(!isRotating){
            yield return null;
        }
        
        while(isRotating){
            if(transform.rotation == targetAngle) break;

            this.transform.rotation = Quaternion.RotateTowards(transform.rotation , targetAngle , Time.deltaTime * speed);
            yield return null;
        }

        if(isRotating){
            yield return new WaitForSeconds(pauseTime);
            goToMin = !goToMin;
            if(goToMin)
                StartCoroutine(RotateNode(minAngleRotation));
            else
                StartCoroutine(RotateNode(maxAngleRotation));
        }

        else
            if(goToMin)
                StartCoroutine(RotateNode(minAngleRotation));
            else
                StartCoroutine(RotateNode(maxAngleRotation));
        
        yield return null;
    }
}
