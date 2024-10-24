using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Entity {
    protected override void Awake(){
        base.Awake();

        isAlive = true;
        Type = Type.ENEMY;
        Initialize();
    }

    protected virtual void Initialize(){}
}
