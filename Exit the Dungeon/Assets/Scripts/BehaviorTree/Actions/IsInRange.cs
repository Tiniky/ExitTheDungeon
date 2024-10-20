using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CL_IsInRange_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Condition Leaf Nodes/IsInRange")]
public class IsInRange : ConditionLeafNode {
    
    private void Awake(){
        NameOfNode = "CheckRange";
        ExecuteMethod = CheckIsInRange;
    }

    private bool CheckIsInRange(){
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Entity entity = target.GetComponent<Entity>();
        return BattleManager.IsTargetInActionRange(entity);
    }
}