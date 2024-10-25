using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour{
    public Vector3 TargetPosition;
    public float moveSpeed;
    public int DB;

    void Start(){
        moveSpeed = 1.5f;
        TargetPosition = Vector3.zero;
        DB = 0;
    }

    void Update(){
        if(TargetPosition != Vector3.zero && transform.position != TargetPosition){
            if(TargetPosition.x < transform.position.x){
                transform.localScale = new Vector3(-1, 1, 1);
            } else {
                transform.localScale = new Vector3(1, 1, 1);
            }
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, moveSpeed * Time.deltaTime);
            DB++;
        } else {
            ResetTarget();
        }
    }

    private void ResetTarget(){
        TargetPosition = Vector3.zero;
    }
}
