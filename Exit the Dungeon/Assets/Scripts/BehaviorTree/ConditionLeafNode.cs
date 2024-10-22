using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionLeafNode : BehaviorNode {
    public bool ShouldBeTrue;
    public delegate bool Func();
    public Func ExecuteMethod;

    public override NodeStatus Execute(){
        Debug.Log("Executing " + NameOfNode);
        if(ExecuteMethod == null){
            Debug.Log(NameOfNode + "' ExecuteMethod is null");
            return NodeStatus.FAILURE;
        }

        bool result = ExecuteMethod();
        Debug.Log("Result of " + NameOfNode + " " + result);
        Debug.Log("ShouldBeTrue: " + ShouldBeTrue);

        if(!ShouldBeTrue){
            result = !result;
        }

        if(result){
            Debug.Log("Condition Check Succeeded");
            return NodeStatus.SUCCESS;
        } else {
            Debug.Log("Condition Check Failed");
            return NodeStatus.FAILURE;
        }
    }
}
