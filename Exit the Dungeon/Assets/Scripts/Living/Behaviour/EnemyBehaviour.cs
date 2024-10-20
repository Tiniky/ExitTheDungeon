using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CreatureBehaviour {
    private EnemyMovement _movement;

    public override void Initialize(int HP){
        _movement = _current.GetComponent<EnemyMovement>();
    }

    public override void TakeDmg(int DMGvalue){
        Entity currentEntity = _current.GetComponent<Entity>();
        currentEntity.HP.Take(DMGvalue);

        if(currentEntity.HP.GetValue() <= 0){
            Debug.Log(currentEntity.EntityName + " died");
            currentEntity.Death();
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
        if(_movement != null){
            Debug.Log("Moving to target: " + target);
            _movement.MoveTo(target);
        }
    }
}
