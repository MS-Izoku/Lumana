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

    void Start(){
        if(invertY)
            cameraLocks *= -1;
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
        ControlCam();
    }
}
