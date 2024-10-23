using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLeafNode : BehaviorNode {
    public delegate NodeStatus Func(Blackboard blackboard);
    public Func ExecuteMethod;

    public ActionLeafNode(string name, Func method){
        NameOfNode = name;
        ExecuteMethod = method;
    }

    public override NodeStatus Execute(){
        Debug.Log("Executing " + NameOfNode);
        if(ExecuteMethod != null){
            return ExecuteMethod(Blackboard);
        }

        return NodeStatus.FAILURE;
    }
}
