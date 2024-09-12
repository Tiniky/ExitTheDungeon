using System;
using System.Collections;
using System.Collections.Generic;

public class Disadvantage {
    public RollType Type {get; set;}
    public string On {get; set;}

    public Disadvantage(RollType type) {
        if (type == RollType.CHECK) {
            throw new ArgumentException("RollType must be CHECK for this constructor.", nameof(type));
        }

        Type = type;
        On = type == RollType.SAVINGTHROW ? "Death" : "";
    }

    public Disadvantage(RollType type, string on) {
        if (type == RollType.ATTACKROLL) {
            throw new ArgumentException("RollType must be ATTACKROLL for this constructor.", nameof(type));
        }

        Type = type;
        On = on;
    }

    //if roll is a nat20 lose disadvantage
}
