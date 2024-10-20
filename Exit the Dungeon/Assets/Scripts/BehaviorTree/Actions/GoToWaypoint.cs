using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AL_GoToWaypoint", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Action Leaf Nodes/GoToWaypoint")]
public class GoToWaypoint : ActionLeafNode {
    
    private void Awake(){
        NameOfNode = "GoToWaypoint";
        ExecuteMethod = ExecuteGoToWaypoint;
    }

    private NodeStatus ExecuteGoToWaypoint(){
        if(IsAgentIDLE()){
            AgentWorking(true);
            
            Vector3 targetPos = Blackboard.GetValue<Vector3>("SelectedWaypoint");
            GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");

            Debug.Log("Going to waypoint: " + targetPos);
            Debug.Log("Current position: " + current.transform.position);

            Blackboard.SetValue("CurrentPos", current.transform.position);
            EnemyBehaviour enemy = current.GetComponent<EnemyBehaviour>();
            enemy.GoToTarget(targetPos);
        } else {
            Vector3 targetPos = Blackboard.GetValue<Vector3>("SelectedWaypoint");
            Vector3 currentPos = Blackboard.GetValue<Vector3>("CurrentPos");
            GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
            Debug.Log("Current position: " + current.transform.position);
            Debug.Log("Saved position: " + currentPos);
            
            if(currentPos == current.transform.position){
                AgentWorking(false);
                return NodeStatus.FAILURE;
            }
            
            if(Vector3.Distance(currentPos, targetPos) < 0.1f){
                AgentWorking(false);
                return NodeStatus.SUCCESS;
            }
        }

        return NodeStatus.RUNNING;
    }
}
