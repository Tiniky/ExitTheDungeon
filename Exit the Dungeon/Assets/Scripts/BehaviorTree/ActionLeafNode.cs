using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLeafNode : BehaviorNode {
    public delegate NodeStatus Func();
    public Func ExecuteMethod;

    public override NodeStatus Execute(){
        Debug.Log("Executing " + NameOfNode);
        if(ExecuteMethod != null){
            return ExecuteMethod();
        }

        return NodeStatus.FAILURE;
    }
}
