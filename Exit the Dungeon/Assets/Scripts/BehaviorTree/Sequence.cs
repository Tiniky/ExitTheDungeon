using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : BehaviorNode {
    
    public Sequence(string name){
        NameOfNode = name;
    }

    public override NodeStatus Execute(){
        Debug.Log("Sequence - Executing " + NameOfNode);

        int childDB = Children.Count;
        Debug.Log("Sequence - Child of child count: " + childDB);

        for(int i = CurrentChild; i < childDB; i++){
            string non = Children[i].NameOfNode;
            Debug.Log("Sequence - Next up Child: " + non);
            NodeStatus childStatus = Children[i].Execute();

            if(childStatus == NodeStatus.FAILURE){
                Debug.Log(non + " status: " + childStatus);
                return NodeStatus.FAILURE;
            }
            
            Debug.Log("need to check next child cause " + non + " status: " + childStatus);
        }

        Debug.Log("all the children was successful");
        return NodeStatus.SUCCESS;
    }
}
