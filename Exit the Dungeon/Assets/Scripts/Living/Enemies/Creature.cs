using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Entity {
    public bool isInRange;
    private bool _wasFightInitialized;

    protected override void Awake(){
        _wasFightInitialized = false;
        base.Awake();

        isAlive = true;
        Type = Type.ENEMY;
        Initialize();
    }

    protected virtual void Initialize(){}

    private void OnTriggerEnter2D(Collider2D collision){

        if(!_wasFightInitialized && collision.gameObject == GameManager.PlayerObj()){
            isInRange = true;
            Debug.Log("Player is in range of Enemy");

            GameManager.HandleRoomDoors(true);
            //BattleManager.Initialize();
            _wasFightInitialized = true;
        }
    }
}
