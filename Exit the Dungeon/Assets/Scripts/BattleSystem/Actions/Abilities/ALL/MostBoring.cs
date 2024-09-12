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

    public override void Activate() {
        //if(GameManager.InFight() && BattleManager.GetKillCountOf(_holder) > 1)
        _holder.AdvantageAdd(RollType.CHECK, MainSkill.LUK, this);
        this.State = AbilityState.ACTIVE;
    }

    public override void Deactivate() {
        _holder.AdvantageExpired(this);
        this.State = AbilityState.COOLDOWN;
    }

    public void Reset() {
        this.State = AbilityState.READY;
        PassiveUIManager.ResetCD(this);
    }
}

//maybe rework dmg up by 0.2, resets on hit