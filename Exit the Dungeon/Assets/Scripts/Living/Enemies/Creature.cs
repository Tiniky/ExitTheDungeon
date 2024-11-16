using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Entity {
    public delegate void ActionFunc(Entity target);
    public List<ActionFunc> Attacks;
    public Dictionary<ActionFunc, int> AttackRange;

    protected override void Awake(){
        base.Awake();

        isAlive = true;
        Type = Type.ENEMY;
        Initialize();
    }

    protected virtual void Initialize(){}
    
    public void UseAttack(Entity target, int attackIndex){
        int roll = Die.Roll(DieType.D20);
        if(roll + 4 >= target.AC.GetValue()){
            Attacks[attackIndex](target);
        } else {
            Debug.Log($"{EntityName} misses {target.EntityName}.");
            LogManager.AddMessage($"{EntityName} misses {target.EntityName}.");
        }
    }

    public int GetAttackRange(int attackIndex){
        return AttackRange[Attacks[attackIndex]];
    }
}
