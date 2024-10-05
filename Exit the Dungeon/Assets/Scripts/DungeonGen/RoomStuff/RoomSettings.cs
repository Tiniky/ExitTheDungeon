using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class RoomSettings : MonoBehaviour {
    public RoomType Type = RoomType.NONE;
    public GameObject TilePrefab;
    public Tilemap Tilemap;
    public GameObject InteractableHolder;
    public Dictionary<Vector2, InteractableTile> InteractablesOfRoom = new Dictionary<Vector2, InteractableTile>();

    public bool ShouldDrawInteractableTiles = false;
    public bool IsPartyMemberSpawnNeeded = false;
    public bool AreEnemySpawnPointsNeeded = false;

    public Vector2 TopLeftCorner;
    public Vector2 BottomRightCorner; 

    //validation bools
    public bool HasAtLeastOneDoor = false;
    public bool WasPlayerSpawnSet = false;
    public bool WasPartyMemberSpawnSet = false;
    public bool WereEnemySpanwsSet = false;

    public void TypeWasUpdated(){
        switch(Type){
            case RoomType.COMBAT:
                ShouldDrawInteractableTiles = true;
                IsPartyMemberSpawnNeeded = false;
                AreEnemySpawnPointsNeeded = true;
                break;
            case RoomType.PRISON:
                ShouldDrawInteractableTiles = true;
                IsPartyMemberSpawnNeeded = true;
                AreEnemySpawnPointsNeeded = true;
                break;
            default:
                ShouldDrawInteractableTiles = false;
                IsPartyMemberSpawnNeeded = false;
                AreEnemySpawnPointsNeeded = false;
                break;
        }
    }

    public void CheckIfDoorWasUpdated(){
        Doorway doorway = gameObject.GetComponent<Doorway>();

        if(doorway == null){
            Debug.Log("null");
        }

        if(doorway.Doors.Count > 0){
            HasAtLeastOneDoor = true;
        } else {
            HasAtLeastOneDoor = false;
        }
    }

    public Tilemap GetTilemap(){
        return Tilemap;
    }

    public GameObject GetTilePrefab(){
        return TilePrefab;
    }

    public void SaveInteractableTiles(Dictionary<Vector2, InteractableTile> tiles){
        InteractablesOfRoom = tiles;
    }

    public Transform GetHolderGameObject(){
        return InteractableHolder.transform;
    }

    public bool ValidateTemplate(){
        bool isValid = true;

        if(!HasAtLeastOneDoor){
            isValid = false;
        }

        if(!WasPlayerSpawnSet){
            isValid = false;
        }

        if(!WasPartyMemberSpawnSet){
            isValid = false;
        }

        if(!WereEnemySpanwsSet){
            isValid = false;
        }

        return isValid;
    }
}
