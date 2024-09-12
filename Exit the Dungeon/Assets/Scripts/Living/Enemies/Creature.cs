using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class Creature : Entity {
    public bool isInRange;
    private bool _wasFightInitialized;
    private SpriteRenderer _renderer;
    private Color _basic;
    public Color highlight, inRange, outOfRange;

    protected override void Awake(){
        _wasFightInitialized = false;
        base.Awake();

        isAlive = true;
        Type = Type.ENEMY;
        Initialize();
        _renderer = GetComponent<SpriteRenderer>();
        _basic = _renderer.color;
    }

    protected virtual void Initialize(){}

    private void OnTriggerEnter2D(Collider2D collision){

        if(!_wasFightInitialized && collision.gameObject.CompareTag("Player")){
            isInRange = true;
            Debug.Log("Player is in range of Ogre");

            BattleManager.Initialize();
            _wasFightInitialized = true;
        }
    }

    private void OnMouseEnter(){
        if ((BattleManager.IsTargetInActionRange(this) || BattleManager.IsTargetInAbilityRange(this)) && BattleState.IsCurrentlyAttacking()) {
            _renderer.color = inRange;
        } else {
            _renderer.color = outOfRange;
        }

        Debug.Log("hovered in");
    }

    private void OnMouseExit(){
        _renderer.color = _basic;

        Debug.Log("hovered out");
    }

    private void OnMouseDown() {
        if((BattleManager.IsTargetInActionRange(this) || BattleManager.IsTargetInAbilityRange(this)) && Input.GetMouseButtonDown(0)){
            BattleManager.SetTargetOfAction(this);
        }
    }

    public async void LightUp() {
        _renderer.color = highlight;
        await Task.Delay(300);
        _renderer.color = _basic;
    }
}
