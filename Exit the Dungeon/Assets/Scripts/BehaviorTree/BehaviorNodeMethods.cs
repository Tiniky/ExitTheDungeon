using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorNodeMethods {
    public static Dictionary<string, ActionLeafNode.Func> ActionMethods = new Dictionary<string, ActionLeafNode.Func>{
        {"SnapToTile", (blackboard) => SnapToTile(blackboard)},
        {"FindWaypoint", (blackboard) => FindWaypoint(blackboard)},
        {"GoToWaypoint", (blackboard) => GoToWaypoint(blackboard)},
        {"FaceThePlayer", (blackboard) => FaceThePlayer(blackboard)},
        {"IntCheckRoll", (blackboard) => RollDieForFlee(blackboard)},
        {"ContinueFighting", (blackboard) => ContinueFighting(blackboard)},
        {"GetFleeDirection", (blackboard) => GetFleeDirection(blackboard)},
        {"FindFleeWaypoint", (blackboard) => FindFleeWaypoint(blackboard)},
        {"GoToFleeWaypoint", (blackboard) => GoToFleeWaypoint(blackboard)},
        {"SelectTarget", (blackboard) => SelectTarget(blackboard)},
        {"PassTurn", (blackboard) => PassTurn(blackboard)},
        {"AttemptMeleeAttack", (blackboard) => AttemptMeleeAttack(blackboard)},
        {"AttemptRangedAttack", (blackboard) => AttemptRangedAttack(blackboard)},
        {"BattleCry", (blackboard) => BattleCry(blackboard)},
        {"StartCharging", (blackboard) => StartCharging(blackboard)},
        {"CastDeathRay", (blackboard) => CastDeathRay(blackboard)}
    };

    public static Dictionary<string, ConditionLeafNode.Func> ConditionMethods = new Dictionary<string, ConditionLeafNode.Func>{
        {"CheckIsCombat", (blackboard) => CheckIsCombat(blackboard)},
        {"CheckIsPreCombat", (blackboard) => CheckIsPreCombat(blackboard)},
        {"CheckIsNotCombat", (blackboard) => CheckIsNotCombat(blackboard)},
        {"IsTheirTurn", (blackboard) => CheckIsTheirTurn(blackboard)},
        {"ShouldFlee", (blackboard) => CheckIfShouldFlee(blackboard)},
        {"CanFlee", (blackboard) => CheckIfCanFlee(blackboard)},
        {"ShouldKeepFighting", (blackboard) => CheckIfShouldKeepFighting(blackboard)},
        {"IsInCloseRange", (blackboard) => IsInCloseRange(blackboard)},
        {"IsCharging", (blackboard) => CheckIsCharging(blackboard)}
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

    public static NodeStatus SnapToTile(Blackboard Blackboard){
        GameObject fighter = Blackboard.GetValue<GameObject>("OwnerObj");
        Vector3 currentPos = fighter.transform.position;
        InteractableTile tile = TileManager.Instance.GetClosestTile(currentPos);
        if(!tile.isEmpty && tile.EntityOnTile() == fighter){
            Debug.Log("Entity is already on the tile.");
        }

        return NodeStatus.SUCCESS;
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
            Blackboard.SetValue("PlayerDirection", -1);
        } else {
            enemy.transform.localScale = new Vector3(1, 1, 1);
            Blackboard.SetValue("PlayerDirection", 1);
        }

        Creature creature = enemy.GetComponent<Creature>();
        if(creature.Size == Size.SMALL){
            TileManager.Instance.SnapToClosestTile(enemy);
        }

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
        InteractableTile tile = TileManager.Instance.GetClosestEmptyTile(newWaypoint);
        tile.IndicateEnemyTarget();
        Blackboard.SetValue("SelectedWaypoint", tile);
        Debug.Log("Fleeing to: " + tile.transform.position);
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus GoToFleeWaypoint(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        InteractableTile tile = Blackboard.GetValue<InteractableTile>("SelectedWaypoint");
        EnemyBehaviour enemy = current.GetComponent<EnemyBehaviour>();
        InteractableTile currentTile = TileManager.Instance.StandsOn(current, 1)[0];
        currentTile.TileOccupation();
        currentTile.ResetColor();
        TileManager.Instance.shouldRepaint = false;
        
        Vector3 targetPos = tile.transform.position + new Vector3(0, 0.8f, 0);
        enemy.GoToTarget(tile.transform.position);
        tile.TileOccupation(current);
        tile.IndicateTurn();
        enemy.StartCoroutine(WaitForEnemyToReachTarget(current, tile));
        Debug.Log("Fled to waypoint: " + tile.transform.position);

        return NodeStatus.SUCCESS;
    }

    private static IEnumerator WaitForEnemyToReachTarget(GameObject enemy, InteractableTile targetTile) {
        // Wait until the enemy reaches the target position
        while (Vector3.Distance(enemy.transform.position, targetTile.transform.position) > 0.1f) {
            Debug.Log("Waiting for enemy to reach target.");
            yield return null;
        }
    }    

    public static NodeStatus SelectTarget(Blackboard Blackboard){
        List<Entity> targets = BattleManager.GetStillAliveAdventurers();
        Entity target = targets[Random.Range(0, targets.Count)];
        Blackboard.SetValue("TargetObj", target.gameObject);

        Debug.Log("Selected target: " + target.EntityName);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus AttemptMeleeAttack(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Creature creature = current.GetComponent<Creature>();
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Entity entity = target.GetComponent<Entity>();

        Debug.Log("Executing melee attack");
        creature.UseAttack(entity, 0);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus AttemptRangedAttack(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Creature creature = current.GetComponent<Creature>();
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Entity entity = target.GetComponent<Entity>();

        Debug.Log("Executing ranged attack");
        creature.UseAttack(entity, 1);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus BattleCry(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Creature creature = current.GetComponent<Creature>();
        LogManager.AddMessage($"{creature.EntityName} is howling ready for battle.");
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus PassTurn(Blackboard Blackboard){
        if(GameManager.InFight()){
            BattleManager.GoNext();

            Debug.Log("Passing turn.");
        }
        
        return NodeStatus.SUCCESS;
    }

    public static NodeStatus StartCharging(Blackboard blackboard){
        LogManager.AddMessage("Boss started charging up a deadly ability.");
        blackboard.SetValue("IsCharging", true);

        return NodeStatus.SUCCESS;
    }

    public static NodeStatus CastDeathRay(Blackboard blackboard){
        List<Entity> targets = BattleManager.GetStillAliveAdventurers();
        for(int i = 0; i < targets.Count; i++){
            Entity target = targets[i];
            target.HP.Take(target.HP.GetValue());
            LogManager.AddMessage("Boss casted a deadly ray of energy.");
        }
        blackboard.SetValue("IsCharging", false);
        return NodeStatus.SUCCESS;
    }

    public static bool CheckIsCombat(Blackboard Blackboard){
        return GameManager.Phase == GamePhase.COMBAT;
    }

    public static bool CheckIsPreCombat(Blackboard Blackboard){
        return GameManager.Phase == GamePhase.INITIATIVE;
    }

    public static bool CheckIsNotCombat(Blackboard Blackboard){
        return GameManager.Phase != GamePhase.COMBAT && GameManager.Phase != GamePhase.INITIATIVE;
    }

    public static bool CheckIsTheirTurn(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();

        bool isCurrentlyTheirTurn = BattleManager.IsTheirTurn(entity);

        if(isCurrentlyTheirTurn){
            Debug.Log("Checking if it's their turn: True");
        }

        return isCurrentlyTheirTurn;
    }

    public static bool CheckIfShouldFlee(Blackboard Blackboard){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();
        int currentHP = entity.HP.GetValue();
        int maxHP = entity.HP.GetMax();

        if(currentHP <= maxHP * 0.15f || IsAlone(Blackboard)){
            Debug.Log("Should flee.");
            return true;
        }

        Debug.Log("Should not flee.");
        return false;
    }

    public static bool CheckIfCanFlee(Blackboard Blackboard){
        int rolled = Blackboard.GetValue<int>("Roll_FLEE");
        Debug.Log(rolled > 10 || IsAlone(Blackboard) ? "Fleeing." : "Too dumb to flee.");
        return rolled > 10;
    }
    
    public static bool CheckIfShouldKeepFighting(Blackboard Blackboard){
        bool status = Blackboard.GetValue<bool>("ShouldKeepFighting");
        Debug.Log(status ? "Should keep fighting." : "Should not keep fighting.");
        return status;
    }

    public static bool IsInCloseRange(Blackboard Blackboard){
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Entity entity = target.GetComponent<Entity>();
        int range = 1;
        Blackboard.SetValue("Range", range);
        bool inRange = BattleManager.IsTargetInRange(entity, range);
        Debug.Log("Checking if target (" + entity.EntityName + ") is in range: "+ range +" "+ inRange);
        return inRange;
    }

    public static bool CheckIsCharging(Blackboard Blackboard){
        return Blackboard.GetValue<bool>("IsCharging");
    }

    public static bool IsAlone(Blackboard Blackboard){
        return BattleManager.IsEnemyAlone();
    }
}