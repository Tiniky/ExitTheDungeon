using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouldKeepFighting : ConditionLeafNode {
    
    private void Awake(){
        NameOfNode = "ShouldKeepFighting";
        ExecuteMethod = CheckIfShouldKeepFighting;
    }

    private bool CheckIfShouldKeepFighting(){
        bool status = Blackboard.GetValue<bool>("ShouldKeepFighting");
        return status;
    }
}