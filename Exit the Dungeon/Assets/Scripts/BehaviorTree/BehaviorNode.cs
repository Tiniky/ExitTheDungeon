using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorNode{
    public List<BehaviorNode> Children = new List<BehaviorNode>();
    protected int CurrentChild;

    public NodeStatus Status { get; protected set; }
    public string NameOfNode { get; protected set; }
    public Blackboard Blackboard { get; set; }

    public virtual void Initialize(Blackboard blackboard){
        Blackboard = blackboard;
        CurrentChild = 0;

        foreach(var child in Children){
            child.Initialize(blackboard);
        }
    }

    public virtual NodeStatus Execute(){
        return NodeStatus.SUCCESS;
    }
}
