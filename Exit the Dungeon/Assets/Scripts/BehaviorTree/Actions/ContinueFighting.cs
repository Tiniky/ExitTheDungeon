using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueFighting : ActionLeafNode {
    
    private void Awake(){
        NameOfNode = "ContinueFighting";
        ExecuteMethod = ExecuteContinueFighting;
    }

    private NodeStatus ExecuteContinueFighting(){
        Blackboard.SetValue("ShouldKeepFighting", true);
        return NodeStatus.SUCCESS;
    }
}
