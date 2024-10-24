using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RootNode : BehaviorNode {
    private Blackboard _blackboard;
    private GameObject _owner;

    struct NodeLevel{
        public BehaviorNode Node;
        public int Level;
    }

    public RootNode(string name){
        NameOfNode = name;
    }

    public override NodeStatus Execute(){
        //Debug.Log("BTRoot - Executing " +  Children[CurrentChild].NameOfNode);
        return Children[CurrentChild].Execute();
    }

    public void SetAgent(GameObject owner){
        NameOfNode = "Root";
        _blackboard = new Blackboard();
        Initialize(_blackboard);
        _owner = owner;
        //Debug.Log("agent's spawn - tree: " + _owner.transform.position);
        _blackboard.SetValue("OwnerObj", _owner);
        _blackboard.SetValue("SpawnPoint", _owner.transform.position);
    }

    public void PrintTree(){
        StringBuilder tree = new StringBuilder();
        Stack<NodeLevel> stack = new Stack<NodeLevel>();
        BehaviorNode currentNode = this;
        stack.Push(new NodeLevel { Node = currentNode, Level = 0 });

        while(stack.Count > 0){
            NodeLevel next = stack.Pop();
            tree.AppendLine(new string('-', next.Level) + next.Node.NameOfNode);
            for(int i = next.Node.Children.Count - 1; i >= 0; i--){
                stack.Push(new NodeLevel { Node = next.Node.Children[i], Level = next.Level + 1 });
            }
        }

        Debug.Log(tree.ToString());
    }
}
