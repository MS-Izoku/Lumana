using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Animator characterAnimator;
    public bool isGrounded = false;

    void Start(){
        characterAnimator = GetComponent<Animator>();
    }

    public void PlayerMovement(){
        float moveHor = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(moveHor , 0 , moveVert).normalized * speed * Time.deltaTime;
        transform.Translate(playerMovement , Space.Self);
    }

    void Update(){
        PlayerMovement();
    }
}
