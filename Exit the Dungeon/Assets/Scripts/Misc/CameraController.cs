using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour{
    private float moveSpeed = 5f;
    public bool isInFight;
    public CinemachineVirtualCamera virtualCamera;
    private float edgeBoundary = 50f;

    void Start(){
        isInFight = GameManager.InFight();
    }

    void Update(){
        isInFight = GameManager.InFight();

        if(isInFight){
            virtualCamera.enabled = false;
            
            float horizontalInput = 0f;
            float verticalInput = 0f;

            //arrow control
            if(Input.GetKey(KeyCode.LeftArrow)){
                horizontalInput = -1f;
            }

            if(Input.GetKey(KeyCode.RightArrow)){
                horizontalInput = 1f;
            }

            if(Input.GetKey(KeyCode.UpArrow)){
                verticalInput = 1f;
            }

            if(Input.GetKey(KeyCode.DownArrow)){
                verticalInput = -1f;
            }

            //mouse control
            Vector3 mousePosition = Input.mousePosition;
            if(mousePosition.x < edgeBoundary) {
                horizontalInput = -1f;
            } else if(mousePosition.x > Screen.width - edgeBoundary) {
                horizontalInput = 1f;
            }

            if(mousePosition.y < edgeBoundary) {
                verticalInput = -1f;
            } else if(mousePosition.y > Screen.height - edgeBoundary) {
                verticalInput = 1f;
            }

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        } else {
            virtualCamera.enabled = true;
        }
    }
}
