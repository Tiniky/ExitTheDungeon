using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Creature {
    protected override void Initialize(){
        EntityName = "Beholder";
        HP = new HitPoints(19, DieType.D10, 76, 180);
        AC = new ArmorClass(18);
        PP = new PassivePerception(12);
        Speed = new Speed(0);
        Size = Size.HUMONGOUS;
        SkillTree = new SkillTree(18, 14, 17, 10);
    }
}
