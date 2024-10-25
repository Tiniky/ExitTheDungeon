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
            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
                Vector3 mousePosition = Input.mousePosition;
                if(mousePosition.x < edgeBoundary){
                    horizontalInput = -1f;
                } else if(mousePosition.x > Screen.width - edgeBoundary){
                    horizontalInput = 1f;
                }

                if(mousePosition.y < edgeBoundary){
                    verticalInput = -1f;
                } else if(mousePosition.y > Screen.height - edgeBoundary){
                    verticalInput = 1f;
                }
            }

            //need to calculate center to be wall - 5f

            Vector3 roomCenter = GameManager.CurrentRoom.RoomObj.transform.position;
            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + movement;
            newPosition.x = Mathf.Clamp(newPosition.x, roomCenter.x - 5f, roomCenter.x + 5f);
            newPosition.y = Mathf.Clamp(newPosition.y, roomCenter.y - 5f, roomCenter.y + 5f);

            transform.position = newPosition;
        } else {
            virtualCamera.enabled = true;
        }
    }
}
