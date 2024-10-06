using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
    public Animator animator;
    public PlayerMovement movement;
    public MoveToTile mtm;
    private bool _wasFightInitialized; 

    void Start(){
        movement = GetComponent<PlayerMovement>();
        mtm = GetComponent<MoveToTile>();
        animator = GetComponent<Animator>();
        _wasFightInitialized = false;
    } 

    void Update(){
        if(animator != null && (movement != null || mtm != null)){
            Vector2 move;

            if(GameManager.InFight()){
                if(!_wasFightInitialized){
                    InitializeFight();
                    _wasFightInitialized = true;
                    
                    move.x = 0;
                    move.y = 0;
                    Debug.Log("nullázvA");
                }

                Vector3 direction = (mtm.targetPosition - transform.position).normalized;
                move.x = Mathf.Clamp(direction.x, -1f, 1f);
                move.y = Mathf.Clamp(direction.y, -1f, 1f);
                
            } else {
                _wasFightInitialized = false;
                animator.SetBool("InFight", false);
                move = movement.move;
            }

            if (move != Vector2.zero) {
                move.Normalize();
            }

            animator.SetFloat("Horizontal", move.x);
            animator.SetFloat("Vertical", move.y);
            animator.SetFloat("MovementSpeed", move.magnitude);
        }
    }

    private void InitializeFight(){
        animator.SetBool("InFight", true);
    }
}