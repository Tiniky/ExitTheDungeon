using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour{
    public Vector3 targetPosition;
    private float moveSpeed;

    void Start(){
        moveSpeed = 3.5f;
        targetPosition = Vector3.zero;
    }

    void Update(){
        if(targetPosition != Vector3.zero && transform.position != targetPosition){
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } else {
            ResetTarget();
        }
    }

    public void MoveTo(Vector3 target){
        targetPosition = target;
    }

    private void ResetTarget(){
        targetPosition = Vector3.zero;
    }
}
