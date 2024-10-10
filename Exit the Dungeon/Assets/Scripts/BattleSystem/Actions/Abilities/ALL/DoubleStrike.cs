using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class DoubleStrike : Ability {
    private Adventurer _holder;
    private Entity _target;

    public DoubleStrike(Adventurer entity){
        this.DisplayName = "Double Strike";
        this.Cooldown = 0;
        this.ActiveRounds = 1;
        this.LastTimeCasted = 0;
        this.Trigger = TriggerType.NONE;
        this.Type = AbilityType.ACTIVE;
        this.Range = AbilityRange.CLOSE;
        this.State = AbilityState.READY;
        this.Effect = AbilityEffect.DAMAGE;
        this.Icon = PrefabManager.IMG_DOUBLE_STRIKE;
        _holder = entity;
        entity.LearnAbility(this);
    }

    public override void Activate() {
        if(BattleState.CanUseAction()){
            BattleState.ActionUsed();
            BattleManager.AbilityChosen(this);
            BattleState.DeclareAttack(true);
            BattleManager.LightUpAllEnemy();
        }
    }

    public override void SetTarget(Entity entity) {
        _target = entity;
        BattleState.DeclareAttack(false);

        int attackRoll = Die.AttackRoll(_holder, true);
        int enemyAC = _target.AC.GetValue();
        string info = GetInfo(attackRoll, enemyAC);
        Announce(info);

        if(attackRoll >= enemyAC){
            ExecuteAttack();
        }
    }

    private void ExecuteAttack() {
        if(_target != null && BattleManager.IsTargetInAbilityRange(_target)) {
            float attackBonus = 0;
            if (_holder is Adventurer adventurer) {
                attackBonus = adventurer.temporaryAttackBonus;
            }
            _target.Behaviour.TakeDmg(Die.Roll(_holder.Melee.DMG, _holder.Melee.DMGmult + 1) + (int)attackBonus);
            
            FightUIManager.UpdateHPFor(_target);
        }

        this.State = AbilityState.COOLDOWN;
    }

    public override bool IsWaitingForTarget(){
        return BattleState.IsCurrentlyAttacking();
    }

    public override void Deactivate() {
        BattleState.Reset();
    }

    private async void Announce(string msg){
        FightUIManager.UpdateFightInfo(msg);
        await Task.Delay(1500);
        FightUIManager.ClearFightInfo();
    }

    private string GetInfo(int attackRoll, int enemyAC){
        StringBuilder sb = new StringBuilder();
        sb.Append(_holder.EntityName);
        sb.Append(" used ");
        sb.Append(this.DisplayName);
        sb.Append(" on ");
        sb.Append(_target.EntityName);
        sb.Append("\n");
        sb.Append("attacker: ");
        sb.Append(attackRoll.ToString());
        sb.Append(" vs enemy: ");
        sb.Append(enemyAC.ToString());

        return sb.ToString();
    }
}