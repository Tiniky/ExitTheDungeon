using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BehaviorNode {

    public Selector(string name){
        NameOfNode = name;
    }

    public override NodeStatus Execute(){
        Debug.Log("Selector - Executing " + NameOfNode);

        int childDB = Children.Count;
        Debug.Log("Selector - Child of child count: " + childDB);

        for(int i = CurrentChild; i < childDB; i++){
            string non = Children[i].NameOfNode;
            Debug.Log("Selector - Next up Child: " + non);
            NodeStatus childStatus = Children[i].Execute();

            if(childStatus == NodeStatus.SUCCESS){
                Debug.Log(non + " status: " + childStatus);
                return NodeStatus.SUCCESS;
            }
            
            Debug.Log("need to check next child cause " + non + " status: " + childStatus);
        }

        Debug.Log("all the children failed");
        return NodeStatus.FAILURE;
    }
}
