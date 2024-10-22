using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaypoint : ActionLeafNode {
    private const float DistanceThreshold = 0.1f;

    public override void Initialize(Blackboard blackboard){
        base.Initialize(blackboard);
        NameOfNode = "GoToWaypoint";
        ExecuteMethod = ExecuteGoToWaypoint;
    }

    private NodeStatus ExecuteGoToWaypoint(){
        Vector3 targetPos = Blackboard.GetValue<Vector3>("SelectedWaypoint");
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Debug.Log("Going to waypoint: " + targetPos);
        Debug.Log("Current position: " + current.transform.position);
        EnemyBehaviour enemy = current.GetComponent<EnemyBehaviour>();
        enemy.GoToTarget(targetPos);
        return NodeStatus.SUCCESS;
    }
}
