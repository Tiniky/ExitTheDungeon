using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorNode : ScriptableObject{
    public NodeStatus Status { get; protected set; }
    public List<BehaviorNode> Children = new List<BehaviorNode>();
    public int CurrentChild = 0;
    public string NameOfNode;

    public BehaviorNode(){}

    public BehaviorNode(string name){
        NameOfNode = name;
    }

    public void AddChild(BehaviorNode child){
        Children.Add(child);
    }

    public virtual NodeStatus Execute(){
        return Children[CurrentChild].Execute();
    }
}
