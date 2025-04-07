using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CreatureBehaviour {
    public EnemyMovement _movement;

    void Update(){
        if(!canMove && _movement.TargetPosition != _current.transform.position){
            _movement.TargetPosition = _current.transform.position;
        }
    }

    public override void TakeDmg(int DMGvalue){
        Entity currentEntity = _current.GetComponent<Entity>();
        currentEntity.HP.Take(DMGvalue);

        GameManager.UpdateDMGDealt(DMGvalue);
        if(currentEntity.HP.GetValue() <= 0){
            Debug.Log(currentEntity.EntityName + " died");
            currentEntity.Death();
            GameManager.UpdateKillCount(BattleManager.GetCurrentFighterInitial());
        }

        UpdateHP(currentEntity);
    }

    public override void AddHeal(int HEALvalue){
        _current.GetComponent<Entity>().HP.Add(HEALvalue);
    }

    public void UpdateHP(Entity entity){
        FightUIManager.UpdateHPFor(entity);
    }

    public void GoToTarget(Vector3 target){
        if(_movement != null && canMove){
            //Debug.Log("Moving to target: " + target);
            _movement.TargetPosition = target;
        }
    }

    public Vector3 GetTarget(){
        return _movement.TargetPosition;
    }

    public void StopMovement(){
        if(_movement != null){
            _movement.TargetPosition = _current.transform.position;
            canMove = false;
        }
    }
}
