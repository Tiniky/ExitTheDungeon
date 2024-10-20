using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CL_CanFlee_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Condition Leaf Nodes/CanFlee")]
public class CanFlee : ConditionLeafNode {
    
    private void Awake(){
        NameOfNode = "CanFlee";
        ExecuteMethod = CheckIfCanFlee;
    }

    private bool CheckIfCanFlee(){
        int rolled = Blackboard.GetValue<int>("Roll_FLEE");
        return rolled >= 10;
    }
}
