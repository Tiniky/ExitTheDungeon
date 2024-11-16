using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorNodeMethods {
    public static Dictionary<string, ActionLeafNode.Func> ActionMethods = new Dictionary<string, ActionLeafNode.Func>{
        {"FindWaypoint", (blackboard) => FindWaypoint(blackboard)},
        {"GoToWaypoint", (blackboard) => GoToWaypoint(blackboard)},
        {"FaceThePlayer", (blackboard) => FaceThePlayer(blackboard)},
        {"DoNothing", (blackboard) => DoNothing(blackboard)},
        {"IntCheckRoll", (blackboard) => RollDieForFlee(blackboard)},
        {"ContinueFighting", (blackboard) => ContinueFighting(blackboard)},
        {"GetFleeDirection", (blackboard) => GetFleeDirection(blackboard)},
        {"FindFleeWaypoint", (blackboard) => FindFleeWaypoint(blackboard)},
        {"GoToFleeWaypoint", (blackboard) => GoToFleeWaypoint(blackboard)},
        {"SelectTarget", (blackboard) => SelectTarget(blackboard)},
        {"SelectAttack", (blackboard) => SelectAttack(blackboard)},
        {"TryCloseGap", (blackboard) => TryCloseGap(blackboard)},
        {"ExecuteAttack", (blackboard) => ExecuteAttack(blackboard)},
        {"PassTurn", (blackboard) => PassTurn(blackboard)}
    };

    public static Dictionary<string, ConditionLeafNode.Func> ConditionMethods = new Dictionary<string, ConditionLeafNode.Func>{
        {"CheckIsCombat", (blackboard) => CheckIsCombat(blackboard)},
        {"IsTheirTurn", (blackboard) => CheckIsTheirTurn(blackboard)},
        {"ShouldFlee", (blackboard) => CheckIfShouldFlee(blackboard)},
        {"CanFlee", (blackboard) => CheckIfCanFlee(blackboard)},
        {"ShouldKeepFighting", (blackboard) => CheckIfShouldKeepFighting(blackboard)},
        {"IsInRange", (blackboard) => CheckIsInRange(blackboard)}
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

    public static NodeStatus FaceThePlayer(Blackboard Blackboard){
        GameObject enemy = Blackboard.GetValue<GameObject>("OwnerObj");
        GameObject player = GameManager.PlayerObj();

        if(enemy.transform.position.x > player.transform.position.x){
            enemy.transform.localScale = new Vector3(-1, 1, 1);
        } else {
            enemy.transform.localScale = new Vector3(1, 1, 1);
        }

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

        Debug.Log("Rolled for Flee: " + rolled);
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus ContinueFighting(Blackboard Blackboard){
        Blackboard.SetValue("ShouldKeepFighting", true);
        Debug.Log("Continuing to fight.");
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus GetFleeDirection(Blackboard Blackboard){
        GameObject enemy = Blackboard.GetValue<GameObject>("OwnerObj");
        GameObject player = GameManager.PlayerObj();

        if(enemy.transform.position.x > player.transform.position.x){
            Blackboard.SetValue("FleeDirection", 1);
        } else {
            Blackboard.SetValue("FleeDirection", -1);
        }

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus FindFleeWaypoint(Blackboard Blackboard){
        GameObject enemy = Blackboard.GetValue<GameObject>("OwnerObj");
        Creature creature = enemy.GetComponent<Creature>();

        int direction = Blackboard.GetValue<int>("FleeDirection");
        int maxStep = creature.Speed.StepsAll();
        Vector3 pos = enemy.transform.position;
        Vector3 newWaypoint = new Vector3(pos.x + (direction * maxStep), pos.y, 0);
        Blackboard.SetValue("SelectedWaypoint", newWaypoint);
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus GoToFleeWaypoint(Blackboard Blackboard){
        Vector3 targetPos = Blackboard.GetValue<Vector3>("SelectedWaypoint");
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        EnemyBehaviour enemy = current.GetComponent<EnemyBehaviour>();
        InteractableTile tile = TileManager.Instance.GetClosestEmptyTile(targetPos);
        targetPos = tile.transform.position + new Vector3(0, 0.5f, 0);
        enemy.GoToTarget(tile.transform.position);
        TileManager.Instance.SnapToClosestTile(current, tile);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus SelectTarget(Blackboard Blackboard){
        List<Entity> targets = BattleManager.GetStillAliveAdventurers();
        Entity target = targets[Random.Range(0, targets.Count)];
        Blackboard.SetValue("TargetObj", target.gameObject);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus SelectAttack(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Creature creature = current.GetComponent<Creature>();
        int attackIndex = Random.Range(0, creature.Attacks.Count);
        Blackboard.SetValue("AttackIndex", attackIndex);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus TryCloseGap(Blackboard Blackboard){
        int range = Blackboard.GetValue<int>("Range");
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Creature creature = current.GetComponent<Creature>();
        int maxStep = creature.Speed.StepsAll();
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Entity entity = target.GetComponent<Entity>();
        
        Vector3 pos = current.transform.position;
        Vector3 targetPos = target.transform.position;
        int distance = (int)Mathf.Abs(pos.x - targetPos.x);
        Debug.Log("Distance between them: " + distance);

        if(distance > range){
            int direction = pos.x > targetPos.x ? -1 : 1;
            Vector3 newWaypoint = new Vector3(pos.x + (direction * maxStep), pos.y, 0);
            Blackboard.SetValue("SelectedWaypoint", newWaypoint);
            return NodeStatus.SUCCESS;
        }
        
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus ExecuteAttack(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Creature creature = current.GetComponent<Creature>();
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Entity entity = target.GetComponent<Entity>();
        int selectedAttack = Blackboard.GetValue<int>("AttackIndex");
        creature.UseAttack(entity, selectedAttack);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus PassTurn(Blackboard Blackboard){
        BattleManager.GoNext();
        
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

        bool isCurrentlyTheirTurn = BattleManager.IsTheirTurn(entity);

        if(isCurrentlyTheirTurn){
            Debug.Log("Checking if it's their turn: True");
        } else{
            Debug.Log("Waiting for their turn.");
        }

        return isCurrentlyTheirTurn;
    }

    public static bool CheckIfShouldFlee(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();
        int currentHP = entity.HP.GetValue();
        int maxHP = entity.HP.GetMax();

        if(currentHP <= maxHP * 0.15f){
            Debug.Log("Should flee.");
            return true;
        }

        Debug.Log("Should not flee.");
        return false;
    }

    public static bool CheckIfCanFlee(Blackboard Blackboard){
        int rolled = Blackboard.GetValue<int>("Roll_FLEE");
        Debug.Log(rolled > 10 ? "Fleeing." : "To dumb to flee.");
        return rolled > 10;
    }
    
    public static bool CheckIfShouldKeepFighting(Blackboard Blackboard){
        bool status = Blackboard.GetValue<bool>("ShouldKeepFighting");
        Debug.Log(status ? "Should keep fighting." : "Should not keep fighting.");
        return status;
    }

    public static bool CheckIsInRange(Blackboard Blackboard){
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Entity entity = target.GetComponent<Entity>();
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Creature creature = current.GetComponent<Creature>();
        int selectedAttack = Blackboard.GetValue<int>("AttackIndex");
        int range = creature.GetAttackRange(selectedAttack);
        Blackboard.SetValue("Range", range);
        Debug.Log("Checking if target (" + entity.EntityName + ") is in range: "+ range +" "+ BattleManager.IsTargetInRange(entity, range));
        return BattleManager.IsTargetInActionRange(entity);
    }
}