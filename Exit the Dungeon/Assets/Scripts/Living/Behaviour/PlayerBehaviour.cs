using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : CreatureBehaviour {
    private Entity entity;
    void Start(){
        entity = gameObject.GetComponent<Entity>();
    }

    public override void TakeDmg(int DMGvalue){
        Entity current = GameManager.Player();
        current.HP.Take(DMGvalue);

        if(current.HP.GetValue() <= 0){
            AbilityManager.Notify(TriggerType.HP_UNDER_0, (Adventurer)entity);
        }

        if(current.HP.GetValue() <= 0){
            current.Death();
        }

        UpdateHP();
    }

    public override void AddHeal(int HEALvalue){
        GameManager.GetPlayerHP().Add(HEALvalue);
        UpdateHP();
    }

    public override void Revive(int HPamount = 1){
        HitPoints hp = GameManager.GetPlayerHP();
        hp.Reset();

        if(HPamount != 1){
            hp.Add(HPamount - 1);
        }

        UpdateHP();
    }

    public void UpdateHP(){
        HealthBarManager.UpdateHPFor(entity);
        FightUIManager.UpdateHPFor(entity);
    }
}
