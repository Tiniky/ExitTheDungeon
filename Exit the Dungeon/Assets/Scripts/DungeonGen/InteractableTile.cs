using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTile : MonoBehaviour {
    public Color baseColor, offsetColor, approveColor, rejectColor, turnindicatorColor;
    private SpriteRenderer _renderer;
    private GameObject _onTheTile;
    public bool isEmpty, isFire;
    private bool _isOffset;

    public void Initialize(bool isOffset){
        isEmpty = true;
        isFire = false;
        _renderer = GetComponent<SpriteRenderer>();
        _isOffset = isOffset;
        ResetColor();
    }

    public bool IsTileInEntityRange(GameObject entityObj, int range){
        float distance = DistanceFromEntity(entityObj);

        Debug.Log(distance);
        return distance <= range;
    }

    private int CalculateManhattanDistance(Vector2 entityPos) {
        Vector2 tilePos = transform.position;
        return (int)Mathf.Abs(tilePos.x - entityPos.x) + (int)Mathf.Abs(tilePos.y - entityPos.y);
    }

    public int DistanceFromEntity(GameObject entityObj){
        Vector2 entityPos = entityObj.transform.position - new Vector3(0f,0.5f,0f);
        return CalculateManhattanDistance(entityPos);
    }

    public void ResetColor(){
        _renderer.color = _isOffset ? baseColor : offsetColor;
    }

    public void TileOccupation(GameObject fighter = null){
        _onTheTile = fighter;
        isEmpty = fighter == null;
    }

    public void IndicateTurn(){
        _renderer.color = turnindicatorColor;
    }

    public GameObject EntityOnTile(){
        return _onTheTile;
    }

    public void PaintTile(bool isWalkable){
        if(isEmpty){
            if(isWalkable){
                _renderer.color = approveColor;
            } else {
                _renderer.color = rejectColor;
            }
        } else if(_onTheTile.GetComponent<Entity>() == BattleManager.CurrentFighter()){
            IndicateTurn();
        } else {
            _renderer.color = rejectColor;
        }
    }

    public void HandleEntity(Entity entity){
        if(isFire){
            entity.Behaviour.TakeDmg(500);
        }
    }
}
