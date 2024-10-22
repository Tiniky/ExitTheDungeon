using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
