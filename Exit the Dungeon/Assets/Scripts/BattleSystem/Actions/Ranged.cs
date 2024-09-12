using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class Ranged : Action {
    private Entity _target, _attacker;
    
    public Ranged() {
        this.ActionType = BasicAction.RANGED;
        this.WasUsed = false;
    }

    public override void Activate() {
        _attacker = BattleManager.CurrentFighter();

        if(_attacker.Ranged == null){
            Debug.Log("nope no ranged weapon");
            return;
        } else {
            if(BattleState.CanUseAction()){
                BattleState.ActionUsed();
                BattleManager.ActionChosen(this);
                BattleState.DeclareAttack(true);
                BattleManager.LightUpAllEnemy();
            }
        }
    }

    public override void SetTarget(Entity entity) {
        _target = entity;
        BattleState.DeclareAttack(false);

        int attackRoll = Die.AttackRoll(_attacker, false);
        int enemyAC = _target.AC.GetValue();
        string info = GetInfo(attackRoll, enemyAC);
        Announce(info);
        if(attackRoll >= enemyAC){
            ExecuteAttack();
        }
    }

    private void ExecuteAttack() {
        if(_target != null && BattleManager.IsTargetInActionRange(_target)) {
            _target.Behaviour.TakeDmg(Die.Roll(_attacker.Ranged.DMG, _attacker.Ranged.DMGmult));
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
