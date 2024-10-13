using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : BehaviorNode {
    public Sequence(string name) : base(name){}

    public override NodeStatus Execute(){
        NodeStatus childStatus = Children[CurrentChild].Execute();
        if(childStatus == NodeStatus.RUNNING){
            return NodeStatus.RUNNING;
        }

        if(childStatus == NodeStatus.FAILURE){
            return NodeStatus.FAILURE;
        }

        CurrentChild++;
        if(CurrentChild >= Children.Count){
            CurrentChild = 0;
            return NodeStatus.SUCCESS;
        }

        return NodeStatus.RUNNING;
    }
}
