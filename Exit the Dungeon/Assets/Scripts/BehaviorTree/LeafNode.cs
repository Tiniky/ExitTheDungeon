using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : BehaviorNode {
    public delegate NodeStatus Tick();
    public Tick ExecuteMethod;

    public Leaf(string name, Tick method){
        NameOfNode = name;
        ExecuteMethod = method;
    }

    public override NodeStatus Execute(){
        if(ExecuteMethod != null){
            return ExecuteMethod();
        }

        return NodeStatus.FAILURE;
    }
}
