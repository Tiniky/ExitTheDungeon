using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private float moveSpeed;
    public Vector2 move;
    private bool shouldStop;

    void Start() {
        moveSpeed = 7.5f;
        shouldStop = false;
    }

    void FixedUpdate() {
        if(!shouldStop){
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = Input.GetAxisRaw("Vertical");
            Vector3 movement = new Vector3(moveSpeed * move.x, moveSpeed * move.y, 0);
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }
    }

    public void Stop(){
        shouldStop = true;
    }

    public void Go(){
        shouldStop = false;
    }
}
