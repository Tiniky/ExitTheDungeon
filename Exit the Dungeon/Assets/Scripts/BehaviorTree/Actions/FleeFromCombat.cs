using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AL_FleeFromCombat", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Action Leaf Nodes/FleeFromCombat")]
public class FleeFromCombat : ActionLeafNode {
    
    private void Awake(){
        NameOfNode = "FleeFromCombat";
        ExecuteMethod = ExecuteFleeFromCombat;
    }

    private NodeStatus ExecuteFleeFromCombat(){
        if(IsAgentIDLE()){
            AgentWorking(true);
        
            Blackboard.SetValue("ShouldKeepFighting", false);

            AgentWorking(false);

            return NodeStatus.SUCCESS;
        } else {
            return NodeStatus.RUNNING;
        }
    }
}
