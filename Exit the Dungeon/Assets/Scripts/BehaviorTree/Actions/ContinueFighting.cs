using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AL_ContinueFighting", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Action Leaf Nodes/ContinueFighting")]
public class ContinueFighting : ActionLeafNode {
    
    private void Awake(){
        NameOfNode = "ContinueFighting";
        ExecuteMethod = ExecuteContinueFighting;
    }

    private NodeStatus ExecuteContinueFighting(){
        if(IsAgentIDLE()){
            AgentWorking(true);

            Blackboard.SetValue("ShouldKeepFighting", true);

            AgentWorking(false);

            return NodeStatus.SUCCESS;
        } else {
            return NodeStatus.RUNNING;
        }
    }
}
