using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BehaviorNode {
    public Selector(string name) : base(name){}

    public override NodeStatus Execute(){
        NodeStatus childStatus = Children[CurrentChild].Execute();
    
        if(childStatus == NodeStatus.RUNNING){
            return NodeStatus.RUNNING;
        }

        if(childStatus == NodeStatus.SUCCESS){
            CurrentChild = 0;
            return NodeStatus.SUCCESS;
        }

        CurrentChild++;
        if(CurrentChild >= Children.Count){
            CurrentChild = 0;
            return NodeStatus.FAILURE;
        }

        return NodeStatus.RUNNING;
    }
}
