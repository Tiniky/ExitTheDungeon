using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour {
    public RootNode Tree;
    public NodeStatus Status;
    public bool isTreeExecuting;

    void Start(){
        if (Tree == null) {
            Debug.LogError("Behavior Tree is not assigned.");
            return;
        }

        Status = NodeStatus.SUCCESS;
        isTreeExecuting = false;

        Debug.Log("agent's spawn: " + gameObject.transform.position);
        Tree.SetAgent(gameObject);
        Tree.PrintTree();
    }

    void Update(){
        if(!isTreeExecuting){
            isTreeExecuting = true;
            StartCoroutine(ExecuteTree());
        }
    }

    private IEnumerator ExecuteTree(){
        Status = Tree.Execute();
        Debug.Log("Tree execution completed with status: " + Status);
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        isTreeExecuting = false;
    }
}
