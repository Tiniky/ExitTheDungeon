using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackResult : ConditionLeafNode {
    
    private void Awake(){
        NameOfNode = "AttackResult";
        ExecuteMethod = CheckIfAttackWasSuccessful;
    }

    private bool CheckIfAttackWasSuccessful(){
        int rolled = Blackboard.GetValue<int>("Roll_ATTACK");
        GameObject target = Blackboard.GetValue<GameObject>("TargetObj");
        Adventurer targetAdv = target.GetComponent<Adventurer>();
        int targetAC = targetAdv.AC.GetValue();
        return rolled >= targetAC;
    }
}