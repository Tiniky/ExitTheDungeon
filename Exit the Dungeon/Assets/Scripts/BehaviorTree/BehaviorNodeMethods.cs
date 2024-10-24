using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorNodeMethods {
    public static Dictionary<string, ActionLeafNode.Func> ActionMethods = new Dictionary<string, ActionLeafNode.Func>{
        {"FindWaypoint", (blackboard) => FindWaypoint(blackboard)},
        {"GoToWaypoint", (blackboard) => GoToWaypoint(blackboard)},
        {"DoNothing", (blackboard) => DoNothing(blackboard)},
        {"IntCheckRoll", (blackboard) => RollDieForFlee(blackboard)},
        {"ContinueFighting", (blackboard) => ContinueFighting(blackboard)},
        {"FleeFromCombat", (blackboard) => FleeFromCombat(blackboard)},
        {"SelectTarget", (blackboard) => SelectTarget(blackboard)},
        {"TryCloseGap", (blackboard) => TryCloseGap(blackboard)},
        {"AttackRoll", (blackboard) => RollDieForAttack(blackboard)},
        {"ExecuteAttack", (blackboard) => ExecuteAttack(blackboard)},
        {"PassTurn", (blackboard) => PassTurn(blackboard)}
    };

    public static Dictionary<string, ConditionLeafNode.Func> ConditionMethods = new Dictionary<string, ConditionLeafNode.Func>{
        {"CheckIsCombat", (blackboard) => CheckIsCombat(blackboard)},
        {"IsTheirTurn", (blackboard) => CheckIsTheirTurn(blackboard)},
        {"ShouldFlee", (blackboard) => CheckIfShouldFlee(blackboard)},
        {"CanFlee", (blackboard) => CheckIfCanFlee(blackboard)},
        {"ShouldKeepFighting", (blackboard) => CheckIfShouldKeepFighting(blackboard)},
        {"IsInRange", (blackboard) => CheckIsInRange(blackboard)},
        {"AttackResult", (blackboard) => CheckIfAttackWasSuccessful(blackboard)}
    };

    private static readonly List<Vector3> OffsetList = new List<Vector3>(){
        new Vector3(-2f, 0, 0),
        new Vector3(-1f, 0, 0),
        new Vector3(1f, 0, 0),
        new Vector3(2f, 0, 0)
    };

    public enum RollType{
        FLEE,
        ATTACK
    }

    public static NodeStatus FindWaypoint(Blackboard Blackboard){
        Vector3 pos = Blackboard.GetValue<Vector3>("SpawnPoint");
        
        float offsetX = OffsetList[Random.Range(0, OffsetList.Count)].x;
        
        Vector3 newWaypoint = new Vector3(pos.x + offsetX, pos.y ,0);
        
        //Debug.Log("Spawnpoint: " + pos);
        //Debug.Log("New waypoint: " + newWaypoint);
        Blackboard.SetValue("SelectedWaypoint", newWaypoint);
        
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus GoToWaypoint(Blackboard Blackboard){
        Vector3 targetPos = Blackboard.GetValue<Vector3>("SelectedWaypoint");
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        //Debug.Log("Going to waypoint: " + targetPos);
        //Debug.Log("Current position: " + current.transform.position);
        EnemyBehaviour enemy = current.GetComponent<EnemyBehaviour>();
        enemy.GoToTarget(targetPos);
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus DoNothing(Blackboard Blackboard){
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus RollDieForFlee(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();
        
        int rolled = Die.AbilityCheck(entity, MainSkill.INT);
        Blackboard.SetValue("Roll_FLEE", rolled);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus RollDieForAttack(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();
        
        int rolled = Die.AttackRoll(entity);
        Blackboard.SetValue("Roll_ATTACK", rolled);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus ContinueFighting(Blackboard Blackboard){
        Blackboard.SetValue("ShouldKeepFighting", true);
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus FleeFromCombat(Blackboard Blackboard){      
        Blackboard.SetValue("ShouldKeepFighting", false);

        //needs implementation
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus SelectTarget(Blackboard Blackboard){
        //needs implementation
        
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus TryCloseGap(Blackboard Blackboard){
        //needs implementation
        
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus ExecuteAttack(Blackboard Blackboard){
        //needs implementation
        
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus PassTurn(Blackboard Blackboard){
        //needs implementation
        
        return NodeStatus.SUCCESS;
    }

    public static bool CheckIsCombat(Blackboard Blackboard){
        //Debug.Log("GamePhase: " + GameManager.Phase.ToString());
        //Debug.Log("CheckIsCombat returning: " + (GameManager.Phase == GamePhase.COMBAT).ToString());
        return GameManager.Phase == GamePhase.COMBAT;
    }

    public static bool CheckIsTheirTurn(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();
        return BattleManager.IsTheirTurn(entity);
    }

    public static bool CheckIfShouldFlee(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();
        int currentHP = entity.HP.GetValue();
        int maxHP = entity.HP.GetMax();

        if(currentHP <= maxHP * 0.15f){
            return true;
        }
        return false;
    }

    public static bool CheckIfCanFlee(Blackboard Blackboard){
        int rolled = Blackboard.GetValue<int>("Roll_FLEE");
        return rolled >= 10;
    }
    
    public static bool CheckIfShouldKeepFighting(Blackboard Blackboard){
        bool status = Blackboard.GetValue<bool>("ShouldKeepFighting");
        return status;
    }

    public static bool CheckIsInRange(Blackboard Blackboard){
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Entity entity = target.GetComponent<Entity>();
        return BattleManager.IsTargetInActionRange(entity);
    }

    public static bool CheckIfAttackWasSuccessful(Blackboard Blackboard){
        int rolled = Blackboard.GetValue<int>("Roll_ATTACK");
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Adventurer targetAdv = target.GetComponent<Adventurer>();
        int targetAC = targetAdv.AC.GetValue();
        return rolled >= targetAC;
    }
}