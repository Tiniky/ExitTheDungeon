using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CL_IsTheirTurn_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Condition Leaf Nodes/IsTheirTurn")]
public class IsTheirTurn : ConditionLeafNode {
    
    private void Awake(){
        NameOfNode = "CheckTurn";
        ExecuteMethod = CheckIsTheirTurn;
    }

    private bool CheckIsTheirTurn(){
        GameObject current = Blackboard.GetValue<GameObject>("OwnerObj");
        Entity entity = current.GetComponent<Entity>();
        return BattleManager.IsTheirTurn(entity);
    }
}
