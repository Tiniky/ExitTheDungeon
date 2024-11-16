using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTile : MonoBehaviour {
    public Color baseColor, offsetColor, approveColor, rejectColor, turnindicatorColor;
    private SpriteRenderer _renderer;
    private GameObject _onTheTile;
    public bool isEmpty, isOffset;
    //private bool _isOffset;

    public void InitializeBackend(bool IsOffset){
        isEmpty = true;
        _renderer = GetComponent<SpriteRenderer>();
        isOffset = IsOffset;
        ResetColor();
    }

    public void Initialize(){
        Vector3 cellPosition = gameObject.transform.position;
        isEmpty = true;
        _renderer = GetComponent<SpriteRenderer>();
        isOffset = (Mathf.Abs(cellPosition.x) + Mathf.Abs(cellPosition.y)) % 2 == 1;
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
        _renderer.color = isOffset ? baseColor : offsetColor;
    }

    public void TileOccupation(GameObject fighter = null){
        _onTheTile = fighter;
        isEmpty = fighter == null;
    }

    public void IndicateTurn(){    
        _renderer.color = turnindicatorColor;
        Debug.Log("tile " + gameObject.transform.position + " was set as turn indicator");
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
}
