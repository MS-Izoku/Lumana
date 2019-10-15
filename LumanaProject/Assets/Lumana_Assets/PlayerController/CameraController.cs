using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotSpeed = 1f;
    public Vector2 cameraLocks = new Vector2(-60 , 60);
    public bool invertY = false;
    public Transform target;
    public Transform player;
    private float mouseX;
    private float mouseY;

    public Transform lockTarget;

    private Vector3 initialLocalTargetPos;
    private Quaternion initialTargetRot;
    private Vector3 initialLocalCameraPos;
    private Quaternion initialCameraRot;

    public bool isLockedOn{
        get{ return lockTarget != null; }
    }


    void Start(){
        SetUpCamera();
        if(invertY)
            cameraLocks *= -1;
        StartCoroutine(ForceLook());
    }

    private void SetUpCamera(){
        initialLocalTargetPos = target.localPosition;
        initialLocalCameraPos = transform.localPosition;
        initialTargetRot = target.transform.rotation;
        initialCameraRot = transform.rotation;
    }

    private void ControlCam(){
        mouseX += Input.GetAxis("Mouse X") * rotSpeed;  // these will need to be refactored for Controllers
        mouseY += Input.GetAxis("Mouse Y") * -rotSpeed;  // these will need to be refactored for Controllers
        mouseY = Mathf.Clamp(mouseY , cameraLocks.x , cameraLocks.y);

        transform.LookAt(target);
        Quaternion targetRot = Quaternion.Euler(0 , mouseX , 0);
        target.rotation = Quaternion.RotateTowards(target.rotation , targetRot , rotSpeed * Time.deltaTime);
        player.rotation = Quaternion.Euler(0 , mouseX , 0);
    }

    void LateUpdate(){
        if(!isLockedOn)
            ControlCam();
    }

    private IEnumerator ForceLook(){
        while(lockTarget == null) yield return null;
        if(lockTarget != null){
            while(lockTarget != null){
                Quaternion targetRot = Quaternion.LookRotation(lockTarget.position - transform.position);
                target.transform.rotation = Quaternion.Slerp(target.transform.rotation , targetRot , Time.deltaTime * rotSpeed);
                yield return null;
            }
        }
       
        // while(!isLockedOn){
        //     if(isLockedOn) break;
        //     Quaternion targetRot = Quaternion.LookRotation(initialLocalCameraPos - transform.position);
        //     target.transform.rotation = Quaternion.Slerp(target.transform.rotation , targetRot , Time.deltaTime * rotSpeed);
        // }
        yield return null;
    }
}
