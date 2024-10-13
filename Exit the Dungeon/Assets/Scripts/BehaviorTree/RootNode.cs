using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : BehaviorNode {
    public RootNode() : base("Root"){}

    struct NodeLevel{
        public BehaviorNode Node;
        public int Level;
    }

    public override NodeStatus Execute(){
        if(Children.Count == 0){
            return NodeStatus.SUCCESS;
        }

        return Children[CurrentChild].Execute();
    }

    public void PrintTree(){
        string tree = "";
        Stack<NodeLevel> stack = new Stack<NodeLevel>();
        BehaviorNode currentNode = this;
        stack.Push(new NodeLevel{Node = currentNode, Level = 0});

        while(stack.Count > 0){
            NodeLevel next = stack.Pop();
            tree += new string('-', next.Level) + next.Node.NameOfNode + "\n";
            for(int i = next.Node.Children.Count - 1; i >= 0; i--){
                stack.Push(new NodeLevel{Node = next.Node.Children[i], Level = next.Level + 1});
            }
        }

        Debug.Log(tree);
    }
}
