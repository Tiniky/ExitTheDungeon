using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothing : ActionLeafNode {
    
    private void Awake(){
        NameOfNode = "DoNothing";
        ExecuteMethod = ExecuteDoNothing;
    }

    private NodeStatus ExecuteDoNothing(){
        return NodeStatus.SUCCESS;
    }
}