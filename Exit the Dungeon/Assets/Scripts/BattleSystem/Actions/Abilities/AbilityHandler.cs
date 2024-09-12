using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour {
    public List<Ability> passives;
    public List<Ability> actives = new List<Ability>();
    public List<AbilityKey> keyPairs = new List<AbilityKey>();

    public void ShowCurrent(Adventurer adventurer){
        passives = new List<Ability>();
        actives = new List<Ability>();
        List<KeyCode> keys = new List<KeyCode>();
        keys = Settings.AbilityKeys;
        
        if(adventurer != null){
            passives = adventurer.GetPassives();
            actives = adventurer.GetActives();

            for(int i = 0; i < actives.Count; i++) {
                actives[i].State = AbilityState.READY;
                keyPairs.Add(new AbilityKey(actives[i], keys[i]));
            }
        }
    }
    
    void Update() {
        foreach(AbilityKey ak in keyPairs) {
            switch(ak.Ability.State){
                case AbilityState.READY:
                    if(Input.GetKeyDown(ak.Key)){
                        ak.Ability.Activate();
                        ak.Ability.State = AbilityState.ACTIVE;
                        ak.ActivateTurn = BattleManager.TurnCounter() + ak.Ability.ActiveRounds;
                    }
                    break;
                case AbilityState.ACTIVE:
                    if(Input.GetKeyDown(ak.Key)){
                        ak.Ability.Deactivate();
                    }
                    
                    if(BattleManager.TurnCounter() == ak.ActivateTurn){
                        ak.Ability.State = AbilityState.COOLDOWN;
                        ak.CooldownTurn = BattleManager.TurnCounter() + ak.Ability.Cooldown;
                    }
                    break;
                case AbilityState.COOLDOWN:
                    if(BattleManager.TurnCounter() == ak.CooldownTurn){
                        ak.Ability.State = AbilityState.READY;
                    }
                    break;
            }
        }
    }
}
