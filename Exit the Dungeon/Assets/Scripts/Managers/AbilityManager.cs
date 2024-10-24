using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityManager {
    public static Ability InitializeAbility(string AbilityName, Adventurer entity){
        switch(AbilityName){
            case "Cure Wounds":
                return new CureWounds(entity);
            case "Double Strike":
                return new DoubleStrike(entity);
            case "Elven Training":
                return new ElvenTraining(entity);
            case "Fire Bolt":
                return new FireBolt(entity);
            case "Mark of Warding":
                return new MarkOfWarding(entity);
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
                if(trigger != TriggerType.LOAD_GAME){
                    LogManager.AddMessage(passive.DisplayName + " from "+ holder.EntityName + " was activated.");
                }
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
