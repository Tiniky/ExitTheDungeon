using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class Attack : Action {
    private Entity _target, _attacker;
    private bool IsGodModeOn;
    
    public Attack() {
        this.ActionType = BasicAction.ATTACK;
        this.WasUsed = false;
    }

    public override void Activate() {
        _attacker = BattleManager.CurrentFighter();
        IsGodModeOn = GameManager.IsGodModeOn;

        if(BattleState.CanUseAction()){
            BattleState.ActionUsed();
            BattleManager.ActionChosen(this);
            BattleState.DeclareTarget(true);
            BattleManager.LightUpAllEnemy();
        }
    }

    public override void SetTarget(Entity entity) {
        _target = entity;
        BattleState.DeclareTarget(false);

        int attackRoll = Die.AttackRoll(_attacker, true);
        int enemyAC = _target.AC.GetValue();
        string info = GetInfo(attackRoll, enemyAC);
        Announce(info);

        if(IsGodModeOn || attackRoll >= enemyAC){
            ExecuteAttack();
        }
    }

    private void ExecuteAttack() {
        if(_target != null && BattleManager.IsTargetInActionRange(_target)) {
            if(IsGodModeOn){
                _target.Behaviour.TakeDmg((int)(_target.HP.GetValue()*0.9f));
            } else {
                float attackBonus = 0;
                if(_attacker is Adventurer adventurer){
                    attackBonus = adventurer.temporaryAttackBonus;
                }
                _target.Behaviour.TakeDmg(Die.Roll(_attacker.Melee.DMG, _attacker.Melee.DMGmult) + (int)attackBonus);
            }

            FightUIManager.UpdateHPFor(_target);
        }

        this.WasUsed = true;
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
        sb.Append(_attacker.EntityName);
        sb.Append(" used ");
        sb.Append(ActionType.ToString());
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
