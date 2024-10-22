using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeFromCombat : ActionLeafNode {
    
    private void Awake(){
        NameOfNode = "FleeFromCombat";
        ExecuteMethod = ExecuteFleeFromCombat;
    }

    private NodeStatus ExecuteFleeFromCombat(){      
        Blackboard.SetValue("ShouldKeepFighting", false);
        return NodeStatus.SUCCESS;
    }
}
