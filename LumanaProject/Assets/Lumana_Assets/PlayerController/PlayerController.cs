using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Animator characterAnimator;
    private CapsuleCollider col;
    public float speed;

    public float jumpForce = 5f;
    public float animatorRunThreshold = 0.05f;
    public float maxSpeed = 10f;
    public float maxBackPedalSpeed = 5f;
    [SerializeField] private float groundCheckLength = 0.1f;

    public bool isRunning{
        get{ return Input.GetAxis("Vertical") > 0; }
        set { characterAnimator.SetBool("Running" , value); }
    }

    private Rigidbody rb;
    private float animatorSpeed{
        get{ return characterAnimator.GetFloat("Vertical");}
        set{ characterAnimator.SetFloat("Vertical" , value);}
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
        
        characterAnimator.SetFloat("Horizontal" , moveHor);
        transform.Translate(playerMovement);
        animatorSpeed = moveVert;
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
        bool res = Physics.Raycast(transform.position + Vector3.up * 0.1f , Vector3.down * groundCheckLength , 1f);
        characterAnimator.SetBool("isGrounded" , res );
        return res;
    }


    void Update(){
        PCPlayerMovement();
        Jump();
    }
}
