using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : Creature {
    protected override void Initialize(){
        EntityName = "Ogre";
        HP = new HitPoints(7, DieType.D10, 21, 59);
        AC = new ArmorClass(11);
        PP = new PassivePerception(8);
        Speed = new Speed(40);
        Size = Size.LARGE;
        SkillTree = new SkillTree(16, 8, 5, 19);
    }
}
