using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InstantiatedRoom {
    public Room Room;
    public GameObject RoomObj;
    public Vector3 TopLeftCorner, BottomRightCorner;
    public int NeighborsNum, NeighborsInstantiated;

    public InstantiatedRoom(Room room, GameObject template, int neighbors){
        Room = room;
        RoomObj = template;
        NeighborsNum = neighbors;
        NeighborsInstantiated = 0;
        CalculateCorners();
    }

    public void AnotherNeighborDone(){
        NeighborsInstantiated++;
    }

    private Vector3 CalculateShift(Vector3 pos1, Vector3 pos2){
        Vector3 shiftedToDoorPos = Vector3.zero; 
        shiftedToDoorPos.x = pos1.x + pos2.x;
        shiftedToDoorPos.y = pos1.y + pos2.y;
        return shiftedToDoorPos;
    }

    public Vector3 CurrentPosition(){
        return RoomObj.transform.position;
    }

    private void CalculateCorners(){
        Vector3 curr = CurrentPosition();
        RoomSettings rs = RoomObj.GetComponent<RoomSettings>();
        BottomRightCorner = CalculateShift(curr, rs.BottomRightCorner);
        TopLeftCorner = CalculateShift(curr, rs.TopLeftCorner);
    }

    public List<Door> GetFreeDoors(){
        Doorway dw = RoomObj.GetComponent<Doorway>();
        return dw.Doors.Where(door => !door.WasUsed).ToList();
    }    
}
