using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityKey {
    public Ability Ability {get; set;}
    public KeyCode Key {get; set;}

    public int ActivateTurn {get; set;}
    public int CooldownTurn {get; set;}

    public AbilityKey(Ability ability, KeyCode key){
        this.Ability = ability;
        this.Key = key;
        this.ActivateTurn = 0;
        this.CooldownTurn = 0;
    }
}
