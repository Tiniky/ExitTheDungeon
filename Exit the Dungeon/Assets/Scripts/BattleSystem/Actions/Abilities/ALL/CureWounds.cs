using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

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
        this.Range = AbilityRange.CLOSE;
        this.State = AbilityState.READY;
        this.Effect = AbilityEffect.HEAL;
        this.Icon = PrefabManager.IMG_CURE_WOUNDS;
        _holder = entity;
        entity.LearnAbility(this);
    }

    public override void Activate(){
        if(BattleState.CanUseAction()){
            BattleState.ActionUsed();
            BattleManager.AbilityChosen(this);
            BattleState.DeclareTarget(true);
            BattleManager.LightUpAllAlly();
        }
    }

    public override void SetTarget(Entity entity){
        _target = entity;
        BattleState.DeclareTarget(false);
        
        string info = GetInfo();
        Announce(info);

        ExecuteHeal();
    }

    private void ExecuteHeal(){
        if(_target != null && BattleManager.IsTargetInAbilityRange(_target)) {
            int healBonus = 0;
            if (_holder is Adventurer adventurer) {
                healBonus = adventurer.temporaryHealBonus;
            }
            _target.Behaviour.AddHeal(Die.Roll(DieType.D8, 2) + healBonus);
            
            FightUIManager.UpdateHPFor(_target);
        }

        this.State = AbilityState.COOLDOWN;
    }

    public override bool IsWaitingForTarget(){
        return BattleState.IsCurrentlyAttacking();
    }

    public override void Deactivate(){
        BattleState.Reset();
    }

    private async void Announce(string msg){
        FightUIManager.UpdateFightInfo(msg);
        await Task.Delay(1500);
        FightUIManager.ClearFightInfo();
    }

    private string GetInfo(){
        StringBuilder sb = new StringBuilder();
        sb.Append(_holder.EntityName);
        sb.Append(" used ");
        sb.Append(this.DisplayName);
        sb.Append(" on ");
        sb.Append(_target.EntityName);
        return sb.ToString();
    }
}
