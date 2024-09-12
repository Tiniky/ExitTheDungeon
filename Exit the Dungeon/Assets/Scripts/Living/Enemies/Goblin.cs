using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Creature {
    protected override void Initialize(){
        EntityName = "Goblin";
        HP = new HitPoints(2, DieType.D6, 0, 7);
        AC = new ArmorClass(15);
        PP = new PassivePerception(9);
        Speed = new Speed(30);
        Size = Size.SMALL;
        SkillTree = new SkillTree(10, 14, 10, 8);
    }
}
