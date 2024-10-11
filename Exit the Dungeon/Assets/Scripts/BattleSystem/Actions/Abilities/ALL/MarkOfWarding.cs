using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkOfWarding : Ability {
    private Adventurer _holder;

    public MarkOfWarding(Adventurer entity){
        this.DisplayName = "Mark of Warding";
        this.Cooldown = 0;
        this.ActiveRounds = 0;
        this.LastTimeCasted = 0;
        this.Trigger = TriggerType.LOAD_GAME;
        this.Type = AbilityType.PASSIVE;
        this.Range = AbilityRange.SELF;
        this.State = AbilityState.READY;
        this.Effect = AbilityEffect.BUFF;
        this.Icon = PrefabManager.IMG_MARK_OF_WARDING;
        _holder = entity;
        entity.LearnAbility(this);
        
        AbilityManager.Notify(TriggerType.LOAD_GAME, (Adventurer)entity);
    }

    public override void Activate(){
        Debug.Log(DisplayName + " activated");
        _holder.ModifyTemporaryHeal(4);
        this.State = AbilityState.ACTIVE;
    }
}
