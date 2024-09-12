using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class Shove : Action {
    private Entity _target, _attacker;
    
    public Shove(){
        this.ActionType = BasicAction.SHOVE;
        this.WasUsed = false;
    }

    public override void Activate() {
        _attacker = BattleManager.CurrentFighter();

        if(BattleState.CanUseAction()){
            BattleState.ActionUsed();
            BattleManager.ActionChosen(this);
            BattleState.DeclareAttack(true);
            BattleManager.LightUpAllEnemy();
        }
    }

    public override void SetTarget(Entity entity) {
        _target = entity;
        BattleState.DeclareAttack(false);

        int attackerSTR = Die.AbilityCheck(_attacker, MainSkill.STR);
        int enemySTR = Die.AbilityCheck(_target, MainSkill.STR);
        string info = GetInfo(attackerSTR, enemySTR);
        Announce(info);
        if(attackerSTR >= enemySTR){
            ExecuteShove();
        }
    }

    private void ExecuteShove() {
        if(_target != null && (_target.Size == Size.SMALL || _target.Size == Size.MEDIUM) && BattleManager.IsTargetInActionRange(_target)) {
            List<InteractableTile> targetTile = TileManager.instance.StandsOn(_target.gameObject, 1);
            Vector3 attackerPosition = _attacker.transform.position;
            Vector3 targetPosition = _target.transform.position;

            if (attackerPosition.x < targetPosition.x) {
                targetPosition += Vector3.right;
            } else if (attackerPosition.x > targetPosition.x) {
                targetPosition += Vector3.left;
            } else if (attackerPosition.y > targetPosition.y) {
                targetPosition += Vector3.down;
            } else if (attackerPosition.y < targetPosition.y) {
                targetPosition += Vector3.up;
            }

            targetTile[0].TileOccupation();
            _target.transform.position = targetPosition;
            TileManager.instance.SnapToClosestTile(_target.gameObject);
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

    private string GetInfo(int attackerSTR, int enemySTR){
        StringBuilder sb = new StringBuilder();
        sb.Append(_attacker.EntityName);
        sb.Append(" used ");
        sb.Append(ActionType.ToString());
        sb.Append(" on ");
        sb.Append(_target.EntityName);
        sb.Append("\n");
        sb.Append("attacker: ");
        sb.Append(attackerSTR.ToString());
        sb.Append(" vs enemy: ");
        sb.Append(enemySTR.ToString());

        return sb.ToString();
    }
}