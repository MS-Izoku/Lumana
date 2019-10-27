using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController_V2 : MonoBehaviour
{
    [SerializeField] private Camera playerCam;

    public float speed;
    CharacterController charController;
    Animator animatorController;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    public float gravity = -10f;

    private void Start(){
        if(gravity >= 0) gravity = -9.8f;

        charController = this.transform.GetComponent<CharacterController>();
        animatorController = this.transform.GetComponent<Animator>();
    }

    void Update(){
        GetMovement();
        LookAtCamera();
    }

    void FixedUpdate(){
        charController.Move(moveVelocity);
    }

    private void GetMovement(){
        Debug.Log("Moving");
        moveInput = new Vector3(Input.GetAxis("Horizontal") , 0 , Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * speed * Time.deltaTime;

        if(!charController.isGrounded)
            moveVelocity.y = gravity * Time.deltaTime;
    }

    private void LookAtCamera(){

    }


}
