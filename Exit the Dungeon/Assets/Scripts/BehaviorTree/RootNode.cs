using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BT_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Behavior Tree Root Node")]
public class RootNode : BehaviorNode {
    private Blackboard _blackboard;
    private GameObject _owner;

    struct NodeLevel{
        public BehaviorNode Node;
        public int Level;
    }

    public override NodeStatus Execute(){
        Debug.Log("Executing " + NameOfNode);

        if(Children.Count == 0){
            return NodeStatus.SUCCESS;
        }

        return Children[CurrentChild].Execute();
    }

    public void SetOwner(GameObject owner, AIBehavior ai){
        NameOfNode = "Root";
        _blackboard = new Blackboard();
        Initialize(_blackboard, ai);
        _owner = owner;
        _blackboard.SetValue("OwnerObj", _owner);
        _blackboard.SetValue("SpawnPoint", _owner.transform.position);
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
