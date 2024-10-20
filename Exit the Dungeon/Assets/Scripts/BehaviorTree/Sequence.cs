using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SQ_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Sequence Node")]
public class Sequence : BehaviorNode {
    public string Name;

    public override void Initialize(Blackboard blackboard, AIBehavior agent){
        base.Initialize(blackboard, agent);
        NameOfNode = Name;
    }

    public override NodeStatus Execute(){
        Debug.Log("Executing " + NameOfNode);
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
