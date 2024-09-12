using System;
using System.Collections;
using System.Collections.Generic;

public class Advantage {
    public RollType Type {get; set;}
    public MainSkill Skill {get; set;}

    public Advantage(RollType type) {
        if (type == RollType.CHECK) {
            throw new ArgumentException("RollType must not be CHECK for this constructor.", nameof(type));
        }

        Type = type;
    }

    public Advantage(RollType type, MainSkill skill) {
        if (type == RollType.ATTACKROLL || type == RollType.SAVINGTHROW) {
            throw new ArgumentException("RollType must not be ATTACKROLL or a SAVINGTHROW for this constructor.", nameof(type));
        }

        Type = type;
        Skill = skill;
    }

    //if roll is a nat1 lose advantage
}
