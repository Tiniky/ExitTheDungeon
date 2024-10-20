using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CL_IsCombat_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Behavior Tree/Condition Leaf Nodes/IsCombat")]
public class IsCombat : ConditionLeafNode {
    
    private void Awake(){
        NameOfNode = "CheckCombat";
        ExecuteMethod = CheckIsCombat;
    }

    private bool CheckIsCombat(){
        return GameManager.Phase == GamePhase.COMBAT;
    }
}
