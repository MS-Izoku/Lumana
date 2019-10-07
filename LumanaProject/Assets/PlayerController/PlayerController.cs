using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public void PlayerMovement(){
        float moveHor = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(moveHor , 0 , moveVert) * speed * Time.deltaTime;
        transform.Translate(playerMovement , Space.Self);
    }

    void Update(){
        PlayerMovement();
    }
}
