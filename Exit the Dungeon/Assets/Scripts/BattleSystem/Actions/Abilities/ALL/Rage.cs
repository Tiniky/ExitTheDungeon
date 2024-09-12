using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : Ability {
    private Adventurer _holder;

    public Rage(Adventurer entity){
        this.DisplayName = "Rage";
        this.Cooldown = 100;
        this.ActiveRounds = 4;
        this.LastTimeCasted = 0;
        this.Trigger = TriggerType.NONE;
        this.Type = AbilityType.ACTIVE;
        this.Range = AbilityRange.SELF;
        this.State = AbilityState.READY;
        this.Effect = AbilityEffect.BUFF;
        this.Icon = PrefabManager.IMG_RAGE;
        _holder = entity;
        entity.LearnAbility(this);
    }

    public override void Activate() {
        _holder.ModifyTemporaryDamage(2);
        _holder.AdvantageAdd(RollType.CHECK, MainSkill.STR, this);
        this.State = AbilityState.ACTIVE;
        AbilityUIManager.OnCA(this);
        Debug.Log("Rage active");
    }

    public override void Deactivate() {
        _holder.ModifyTemporaryDamage(0);
        _holder.AdvantageExpired(this);
        this.State = AbilityState.COOLDOWN;
        AbilityUIManager.ResetCA(this);
        AbilityUIManager.OnCD(this);
        Debug.Log("Rage inactive");
    }

    public void Reset() {
        this.State = AbilityState.READY;
        AbilityUIManager.ResetCD(this);
    }
}