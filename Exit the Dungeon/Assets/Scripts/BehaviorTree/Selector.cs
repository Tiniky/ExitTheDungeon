using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "S_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Selector Node")]
public class Selector : BehaviorNode {

    private void Awake(){
        NameOfNode = "Select Strategy";
    }

    public override NodeStatus Execute(){
        Debug.Log("Executing " + NameOfNode);
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
