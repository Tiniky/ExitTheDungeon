using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CL_ShouldFlee_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Condition Leaf Nodes/ShouldFlee")]
public class ShouldFlee : ConditionLeafNode {
    
    private void Awake(){
        NameOfNode = "ShouldFlee";
        ExecuteMethod = CheckIfShouldFlee;
    }

    private bool CheckIfShouldFlee(){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();
        int currentHP = entity.HP.GetValue();
        int maxHP = entity.HP.GetMax();

        if(currentHP <= maxHP * 0.15f){
            return true;
        }
        return false;
    }
}