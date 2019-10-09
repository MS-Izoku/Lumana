using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Animator characterAnimator;
    private CapsuleCollider col;
   // public bool isGrounded = false;
    public float speed;

    public float jumpForce = 5f;
    public float animatorRunThreshold = 0.05f;
    public float maxSpeed = 10f;
    public float maxBackPedalSpeed = 5f;

    public float maxJumpDrag = 5f;

    private Rigidbody rb;
    private float animatorSpeed{
        get{ return characterAnimator.GetFloat("Speed");}
        set{ characterAnimator.SetFloat("Speed" , value);}
    }


    void Start(){
        characterAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    private void PCPlayerMovement(){
        float moveHor = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");

        Vector3 playerMovement = new Vector3(moveHor , 0 , moveVert).normalized * speed * Time.deltaTime;
        transform.Translate(playerMovement , Space.Self);
        animatorSpeed = moveVert;
        // if(animatorSpeed > animatorRunThreshold && animatorSpeed < -animatorRunThreshold) isRunning = false;
        // else isRunning = true;
    }

    //gravity adjusted to -26 at the time of making this
    private void Jump(){
        if(isGrounded()){
            if(Input.GetKeyDown("space") ){
                Vector3 jumpForward = transform.forward * animatorSpeed * (jumpForce * 0.2f);
                Vector3 jumpUp = Vector3.up * jumpForce;
                Vector3 jumpVector = jumpForward + jumpUp;
                rb.AddForce(jumpVector , ForceMode.Impulse);
            }
        }
    }

    private bool isGrounded(){
        return Physics.Raycast(transform.position + Vector3.up * 0.1f , Vector3.down * 0.1f , 1f);
    }


    void Update(){
        PCPlayerMovement();
        Jump();
    }
}
