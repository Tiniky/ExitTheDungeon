using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelentlessEndurance : Ability {
    private Adventurer _holder;

    public RelentlessEndurance(Adventurer entity){
        this.DisplayName = "Relentless Endurance";
        this.Cooldown = 100;
        this.ActiveRounds = 0;
        this.LastTimeCasted = 0;
        this.Trigger = TriggerType.HP_UNDER_50;
        this.Type = AbilityType.PASSIVE;
        this.Range = AbilityRange.SELF;
        this.State = AbilityState.READY;
        this.Effect = AbilityEffect.BUFF;
        this.Icon = PrefabManager.IMG_RELENTLESS_ENDURANCE;
        _holder = entity;
        entity.LearnAbility(this);
    }

    public override void Activate() {
        Debug.Log(DisplayName);
        this.State = AbilityState.COOLDOWN;
        PassiveUIManager.OnCD(this);
        Adventurer adventurer = _holder.gameObject.GetComponent<Adventurer>();
        adventurer.ModifyTemporaryDamage(Die.Roll(DieType.D10));
    }

    public void Reset() {
        this.State = AbilityState.READY;
        PassiveUIManager.ResetCD(this);
    }
}