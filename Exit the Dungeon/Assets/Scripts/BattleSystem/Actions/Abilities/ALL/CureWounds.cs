using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureWounds : Ability {
    private Adventurer _holder;
    private Entity _target;

    public CureWounds(Adventurer entity){
        this.DisplayName = "Cure Wounds";
        this.Cooldown = 1;
        this.ActiveRounds = 0;
        this.LastTimeCasted = 0;
        this.Trigger = TriggerType.NONE;
        this.Type = AbilityType.ACTIVE;
        this.Range = AbilityRange.SELF;
        this.State = AbilityState.READY;
        this.Effect = AbilityEffect.HEAL;
        this.Icon = PrefabManager.IMG_CURE_WOUNDS;
        _holder = entity;
        entity.LearnAbility(this);
    }

    public override void Activate() {
        if(BattleState.CanUseAction()){
            BattleState.ActionUsed();
            BattleManager.AbilityChosen(this);
            BattleState.DeclareAttack(true);
            BattleManager.LightUpAllAlly();
        }
    }
}
