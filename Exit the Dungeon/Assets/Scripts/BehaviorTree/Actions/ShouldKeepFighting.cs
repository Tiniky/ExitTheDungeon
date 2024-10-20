using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CL_ShouldKeepFighting_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Condition Leaf Nodes/ShouldKeepFighting")]
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