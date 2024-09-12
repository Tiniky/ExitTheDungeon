using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionManager {
    public static void CleanUpAfterTurn(Action chosenAction){
        if(chosenAction == null){
            return;
        }

        if(chosenAction.ActionType == BasicAction.DASH){
            Entity entity = BattleManager.CurrentFighter();
            entity.Speed.Dash(false);
        }

        if((chosenAction.ActionType == BasicAction.ATTACK || chosenAction.ActionType == BasicAction.RANGED || chosenAction.ActionType == BasicAction.SHOVE) && chosenAction.IsWaitingForTarget()){
            chosenAction.Deactivate();
        }
    }
}
