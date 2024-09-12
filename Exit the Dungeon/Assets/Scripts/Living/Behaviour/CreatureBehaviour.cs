using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBehaviour : MonoBehaviour{
    public GameObject _current;
    private bool canMove;
    
    void Start(){
        canMove = true;
        _current = gameObject;
    }

    void Update(){
        UpdateBehaviour();
    }

    public virtual void TakeDmg(int DMGvalue) {}
    public virtual void AddHeal(int HEALvalue) {}
    public virtual void Initialize(int HP) {}
    public virtual void Revive(int HPamount = 1) {}
    public virtual void UpdateBehaviour() {}
    

    public bool CheckIfCreatureCanMove(){
        return canMove;
    }

    public void AllowMovement(){
        canMove = true;
    }

    public void DenyMovement(){
        canMove = false;
    }
}
