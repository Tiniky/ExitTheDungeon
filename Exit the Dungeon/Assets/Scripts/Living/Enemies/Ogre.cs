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
    
        Attacks = new List<ActionFunc>(){};
        AttackRange = new Dictionary<ActionFunc, int>();
        Attacks.Add(UseGreatclub);
        AttackRange.Add(UseGreatclub, 2);
        Attacks.Add(SmashGreatclub);
        AttackRange.Add(SmashGreatclub, 2);
    }

    private void UseGreatclub(Entity target){
        int damage = Die.Roll(DieType.D6, 2);
        target.Behaviour.TakeDmg(damage);
        Debug.Log($"{EntityName} hits {target.EntityName} with a greatclub for {damage} damage.");
        LogManager.AddMessage($"{EntityName} hits {target.EntityName} with a greatclub for {damage} damage.");
    }

    private void SmashGreatclub(Entity target){
        int damage = Die.Roll(DieType.D8, 2);
        target.Behaviour.TakeDmg(damage);
        Debug.Log($"{EntityName} smashes {target.EntityName} with a greatclub for {damage} damage.");
        LogManager.AddMessage($"{EntityName} smashes {target.EntityName} with a greatclub for {damage} damage.");
    }
}
