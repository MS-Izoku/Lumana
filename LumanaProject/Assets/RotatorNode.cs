using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorNode : MonoBehaviour
{
    public bool isRotating = false;
    [Range(5f , 100f)]public float speed = 5f;
    public float pauseTime = 0.5f;
    public float minAngle = -90;
    public float maxAngle = 90;
    public float yAdjust = 1f;

    [HideInInspector] public Vector3 adjustedPos;
    [HideInInspector] public Vector3 minTarget;
    [HideInInspector] public Vector3 maxTarget;
    [HideInInspector] public Quaternion minAngleRotation;
    [HideInInspector] public Quaternion maxAngleRotation;
    [HideInInspector] public Vector3 target;

    public bool goToMin = true;
    private bool gameStart = false;
    
    void Start()
    {
        gameStart = true;
        Setup();
        
        if(goToMin)
        {
            transform.rotation = Quaternion.Euler(0 , minAngle , 0) * transform.rotation;
            StartCoroutine(RotateNode(minAngleRotation));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0 , maxAngle , 0) * transform.rotation;
            StartCoroutine(RotateNode(maxAngleRotation));
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        
        if(!gameStart)
            Setup();

        Gizmos.DrawLine(adjustedPos , minTarget);
        Gizmos.DrawWireSphere(minTarget , 0.5f);
        Gizmos.DrawLine(adjustedPos , maxTarget);
        Gizmos.DrawWireSphere(maxTarget , 0.5f);
    }

    public virtual void Setup(){
        target = transform.rotation * (Vector3.forward * 10f);   // GUI
        adjustedPos = transform.position + (Vector3.up * yAdjust);
        Quaternion minAngleRotationEuler = Quaternion.Euler(0 , minAngle , 0);
        Quaternion maxAngleRotationEuler = Quaternion.Euler(0 , maxAngle , 0);

        minAngleRotation = Quaternion.AngleAxis(minAngle , transform.up) * transform.rotation;
        maxAngleRotation = Quaternion.AngleAxis(maxAngle , transform.up) * transform.rotation;
        minTarget = minAngleRotation * target + transform.position;   // GUI
        maxTarget = maxAngleRotation * target + transform.position; // GUI
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
