using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostBoring : Ability {
    private Adventurer _holder;

    public MostBoring(Adventurer entity){
        this.DisplayName = "Most Boring";
        this.Cooldown = 1;
        this.ActiveRounds = 5;
        this.LastTimeCasted = 0;
        this.Trigger = TriggerType.KILLED_A_CREATURE;
        this.Type = AbilityType.PASSIVE;
        this.Range = AbilityRange.SELF;
        this.State = AbilityState.READY;
        this.Effect = AbilityEffect.BUFF;
        this.Icon = PrefabManager.IMG_BORING;
        _holder = entity;
        entity.LearnAbility(this);
    }

    public override void Activate(){
        Debug.Log(DisplayName + " activated");
        _holder.ModifyTemporaryDamage(0.05f, false);
    }
}
