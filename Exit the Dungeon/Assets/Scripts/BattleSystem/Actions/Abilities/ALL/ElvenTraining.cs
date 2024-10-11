using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElvenTraining : Ability {
    private Adventurer _holder;

    public ElvenTraining(Adventurer entity){
        this.DisplayName = "Elven Training";
        this.Cooldown = 0;
        this.ActiveRounds = 0;
        this.LastTimeCasted = 0;
        this.Trigger = TriggerType.LOAD_GAME;
        this.Type = AbilityType.PASSIVE;
        this.Range = AbilityRange.SELF;
        this.State = AbilityState.READY;
        this.Effect = AbilityEffect.BUFF;
        this.Icon = PrefabManager.IMG_ELVEN_TRAINING;
        _holder = entity;
        entity.LearnAbility(this);
        
        AbilityManager.Notify(TriggerType.LOAD_GAME, (Adventurer)entity);
    }

    public override void Activate(){
        Debug.Log(DisplayName + " activated");
        this.State = AbilityState.ACTIVE;
        _holder.AdvantageAdd(RollType.ATTACKROLL, this);
    }
}
