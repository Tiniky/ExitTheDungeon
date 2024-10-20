using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour {
    public RootNode Tree;
    public NodeStatus Status {get; private set;}
    private ActionState _state = ActionState.IDLE;
    private WaitForSeconds _wait;

    void Start(){
        Tree.SetOwner(gameObject, this);
        _wait = new WaitForSeconds(Random.Range(0.1f, 1f));
        StartCoroutine("ExecuteTree");
        //Tree.PrintTree();
    }

    public ActionState GetState(){
        return _state;
    }

    public void DoAction(){
        if(_state == ActionState.IDLE){
            Debug.Log("Working...");
            _state = ActionState.WORKING;
        }
    }

    public void FinishAction(){
        if(_state == ActionState.WORKING){
            Debug.Log("Finished!");
            _state = ActionState.IDLE;
        }
    }

    IEnumerator ExecuteTree(){
        while(true){
            Status = Tree.Execute();
            Debug.Log("Status: " + Status);
            yield return _wait;
        }
    }
}
