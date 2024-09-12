using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityManager {
    public static Ability InitializeAbility(string AbilityName, Adventurer entity){
        switch(AbilityName){
            case "Double Strike":
                return new DoubleStrike(entity);
            case "Most Boring":
                return new MostBoring(entity);
            case "Rage":
                return new Rage(entity);
            case "Relentless Endurance":
                return new RelentlessEndurance(entity);
            case "Sneak Attack":
                return new SneakAttack(entity);
            default:
                return null;
        }
    }

    public static void Notify(TriggerType trigger, Adventurer holder) {
        List<Ability> passives = holder.GetPassives();
        foreach(Ability passive in passives){
            if(passive.Trigger == trigger && passive.State == AbilityState.READY){
                Debug.Log("Activating passive");
                passive.Activate();
            }
        }
    }

    public static void CleanUpAfterTurn(Ability chosenAbility){
        if(chosenAbility == null){
            return;
        }

        if(chosenAbility.Effect == AbilityEffect.DAMAGE && chosenAbility.IsWaitingForTarget()){
            chosenAbility.Deactivate();
        }
    }
}
