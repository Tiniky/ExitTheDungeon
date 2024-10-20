using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AL_FindWaypoint", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Action Leaf Nodes/FindWaypoint")]
public class FindWaypoint : ActionLeafNode {
    
    private void Awake(){
        NameOfNode = "FindWaypoint";
        ExecuteMethod = ExecuteFindWaypoint;
    }

    private NodeStatus ExecuteFindWaypoint(){
        if(IsAgentIDLE()){
            AgentWorking(true);
        
            Vector3 pos = Blackboard.GetValue<Vector3>("SpawnPoint");
            float offsetX = Random.Range(-2f, 2f);
            float offsetY = Random.Range(-2f, 2f);
            Vector3 newWaypoint = new Vector3(pos.x + offsetX, pos.y + offsetY, 0);
            
            Debug.Log("New waypoint: " + newWaypoint);
            
            Blackboard.SetValue("SelectedWaypoint", newWaypoint);
            AgentWorking(false);

            return NodeStatus.SUCCESS;
        } else {
            return NodeStatus.RUNNING;
        }
    }
}
