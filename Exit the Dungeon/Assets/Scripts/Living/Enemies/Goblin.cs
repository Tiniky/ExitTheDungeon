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
    
        Attacks = new List<ActionFunc>(){};
        AttackRange = new Dictionary<ActionFunc, int>();
        Attacks.Add(UseScimitar);
        AttackRange.Add(UseScimitar, 1);
        Attacks.Add(UseShortbow);
        AttackRange.Add(UseShortbow, 10);
    }

    private void UseScimitar(Entity target){
        int damage = Die.Roll(DieType.D6) + 2;
        target.Behaviour.TakeDmg(damage);
        Debug.Log($"{EntityName} hits {target.EntityName} with a scimitar for {damage} damage.");
        LogManager.AddMessage($"{EntityName} hits {target.EntityName} with a scimitar for {damage} damage.");
    }

    private void UseShortbow(Entity target){
        int damage = Die.Roll(DieType.D6);
        target.Behaviour.TakeDmg(damage);
        Debug.Log($"{EntityName} hits {target.EntityName} with a shortbow for {damage} damage.");
        LogManager.AddMessage($"{EntityName} hits {target.EntityName} with a shortbow for {damage} damage.");
    }
}
