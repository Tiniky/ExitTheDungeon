using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour {
    private RootNode _tree;
    private ActionState _state = ActionState.IDLE;
    private NodeStatus _status = NodeStatus.RUNNING;
    private WaitForSeconds _wait;

    void Start(){
        _tree = new RootNode();
        _wait = new WaitForSeconds(Random.Range(0.1f, 1f));
        StartCoroutine("ExecuteTree");
        //_tree.PrintTree();
    }

    private NodeStatus DoAction(){
        if(_state == ActionState.IDLE){
            Debug.Log("Working...");
            _state = ActionState.WORKING;
            return NodeStatus.RUNNING;
        }

        return NodeStatus.FAILURE;
    }

    IEnumerator ExecuteTree(){
        while(true){
            _status = _tree.Execute();
            yield return _wait;
        }
    }
}
