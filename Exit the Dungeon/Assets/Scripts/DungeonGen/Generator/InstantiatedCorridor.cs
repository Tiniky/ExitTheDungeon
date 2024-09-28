using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InstantiatedCorridor {
    public InstantiatedRoom From, To;
    public DoorDirection FromDirection, ToDirection; 
    public GameObject CorridorObj;
    public Vector3 TopLeftCorner, BottomRightCorner;

    public InstantiatedCorridor(GameObject template, InstantiatedRoom room, DoorDirection toDir){
        From = room;
        To = null;
        CorridorObj = template;
        ToDirection = toDir;
        FromDirection = GetOppositeDir(toDir);
        CloseDoor(FromDirection);
        CalculateCorners();
    }

    private DoorDirection GetOppositeDir(DoorDirection dir){
        if(dir == DoorDirection.UP){
            return DoorDirection.DOWN;
        } else if(dir == DoorDirection.LEFT){
            return DoorDirection.RIGHT;
        } else if(dir == DoorDirection.DOWN){
            return DoorDirection.UP;
        } else{
            return DoorDirection.LEFT;
        }
    }

    public bool HasEmptyEnd(){
        return To == null;
    }

    private Vector3 CalculateShift(Vector3 pos1, Vector3 pos2){
        Vector3 shiftedToDoorPos = Vector3.zero; 
        shiftedToDoorPos.x = pos1.x + pos2.x;
        shiftedToDoorPos.y = pos1.y + pos2.y;
        return shiftedToDoorPos;
    }

    public Vector3 ShiftedToDoorPos(){
        Doorway dw = CorridorObj.GetComponent<Doorway>();
        Door d = dw.Doors.FirstOrDefault(door => door.Direction == ToDirection);
        return CalculateShift(CurrentPosition(), d.PossibleStartPosition);
    }

    public Vector3 CurrentPosition(){
        return CorridorObj.transform.position;
    }

    private void CalculateCorners(){
        Vector3 curr = CurrentPosition();
        RoomSettings rs = CorridorObj.GetComponent<RoomSettings>();
        BottomRightCorner = CalculateShift(curr, rs.BottomRightCorner);
        TopLeftCorner = CalculateShift(curr, rs.TopLeftCorner);
    }

    private void CloseDoor(DoorDirection dir, InstantiatedRoom room = null){
        if(room == null){
            Doorway dw = CorridorObj.GetComponent<Doorway>();
            Door door = dw.Doors.FirstOrDefault(door => door.Direction == dir);
            door.WasUsed = true;
        } else {
            Doorway dw = room.RoomObj.GetComponent<Doorway>();
            Door door = dw.Doors.FirstOrDefault(door => door.Direction == dir);
            if(door != null){
                door.WasUsed = true;
            }
        }
    }

    public void CloseUpDoors(InstantiatedRoom room){
        To = room;
        CloseDoor(ToDirection);
        CloseDoor(FromDirection, To);
    }
}
