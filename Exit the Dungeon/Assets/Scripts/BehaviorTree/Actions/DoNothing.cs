using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AL_DoNothing", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Action Leaf Nodes/DoNothing")]
public class DoNothing : ActionLeafNode {
    
    private void Awake(){
        NameOfNode = "DoNothing";
        ExecuteMethod = ExecuteDoNothing;
    }

    private NodeStatus ExecuteDoNothing(){
        return NodeStatus.SUCCESS;
    }
}