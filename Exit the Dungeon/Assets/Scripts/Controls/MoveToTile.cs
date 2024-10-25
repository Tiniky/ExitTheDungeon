using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveToTile : MonoBehaviour {
   
    public Vector3 mousePosition, targetPosition;
    public Camera mainCam;
    private float moveSpeed;
    private InteractableTile currentTile, hoveredTile, lastTile;
    private Entity entity;

    void OnEnable(){
        currentTile = null;
        entity = gameObject.GetComponent<Adventurer>();
        targetPosition = transform.position;
        mainCam = GameObject.FindGameObjectWithTag("CAM").GetComponent<Camera>();
        moveSpeed = 7.5f;
    }

    void Update(){
        if(GameManager.Phase == GamePhase.COMBAT){
            if(BattleManager.IsTheirTurn(entity)){
                BattleManager.MovementRangeOf(entity);

                if(Input.GetMouseButtonDown(1)){
                    mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = transform.position.z;
                    Debug.Log(mousePosition);

                    if(currentTile == null){
                        currentTile = TileManager.Instance.GetClosestTile(transform.position);
                        lastTile = TileManager.Instance.GetClosestTile(transform.position);
                    }

                    currentTile = TileManager.Instance.GetClosestTile(transform.position);
                    hoveredTile = TileManager.Instance.GetClosestTile(mousePosition);
                    Debug.Log(hoveredTile.transform.position);
                    
                    if(hoveredTile.IsTileInEntityRange(gameObject, entity.Speed.StepsLeft()) && hoveredTile.isEmpty){
                        currentTile.TileOccupation();
                        currentTile.ResetColor();
                        TileManager.Instance.shouldRepaint = false;

                        hoveredTile.TileOccupation(gameObject);
                        hoveredTile.IndicateTurn();
                        targetPosition = hoveredTile.transform.position + new Vector3(0,0.5f,0);
                        Debug.Log(targetPosition);
                        FightUIManager.UpdateMovementFor(entity, hoveredTile.DistanceFromEntity(gameObject));
                    }

                }

                if(transform.position != targetPosition){
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                } else {
                    TileManager.Instance.shouldRepaint = true;
                }
            }
        }
    }
}