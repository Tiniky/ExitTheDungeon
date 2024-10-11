using System;
using System.Collections;
using System.Collections.Generic;

public class Advantage {
    public RollType Type {get; set;}
    public MainSkill Skill {get; set;}

    public Advantage(RollType type) {
        Type = type;
    }

    public Advantage(RollType type, MainSkill skill) {
        Type = type;
        Skill = skill;
    }

    //if roll is a nat1 lose advantage
}
