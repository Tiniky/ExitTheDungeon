using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCombat : ConditionLeafNode {
    public override void Initialize(Blackboard blackboard){
        base.Initialize(blackboard);
        NameOfNode = "CheckCombat";
        ExecuteMethod = CheckIsCombat;
    }

    private bool CheckIsCombat(){
        Debug.Log("GamePhase: " + GameManager.Phase.ToString());
        Debug.Log("CheckIsCombat returning: " + (GameManager.Phase == GamePhase.COMBAT).ToString());
        return GameManager.Phase == GamePhase.COMBAT;
    }
}
