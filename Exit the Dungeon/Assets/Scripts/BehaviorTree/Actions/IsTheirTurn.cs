using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
