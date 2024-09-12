using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings{
    // ROOM SETTINGS
    public const int maxChildCorridors = 3;

    // KEY CONTROLS
    public static KeyCode INVENTORY = KeyCode.Tab;
    public static KeyCode PASSTURN = KeyCode.Return;
    public static KeyCode ATTACK = KeyCode.A;
    public static KeyCode DASH = KeyCode.S;
    public static KeyCode RANGED = KeyCode.D;
    public static KeyCode SHOVE = KeyCode.F;
    public static KeyCode ABILITY1 = KeyCode.Q;
    public static KeyCode ABILITY2 = KeyCode.W;
    public static KeyCode ABILITY3 = KeyCode.E;
    public static KeyCode ABILITY4 = KeyCode.R;

    public static KeyCode GetKeyCodeOfAction(string actionName){
        switch(actionName){
            case "ATTACK":
                return ATTACK;
            case "DASH":
                return DASH;
            case "RANGED":
                return RANGED;
            case "SHOVE":
                return SHOVE;
            default:
                return KeyCode.Escape;
        }
    }

    public static KeyCode GetKeyCodeOfAbility(int index){
        switch(index){
            case 0:
                return ABILITY1;
            case 1:
                return ABILITY2;
            case 2:
                return ABILITY3;
            case 3:
                return ABILITY4;
            default:
                return KeyCode.Escape;
        }
    }

    public static List<KeyCode> AbilityKeys { get; private set; }
    static Settings() {
        AbilityKeys = new List<KeyCode> {
            ABILITY1,
            ABILITY2,
            ABILITY3,
            ABILITY4,
        };
    }
}


