using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InstantiatedRoom {
    public Room Room { get; private set; }
    public GameObject RoomObj { get; private set; }
    public Vector3 TopLeftCorner { get; private set; }
    public Vector3 BottomRightCorner { get; private set; }
    public int NeighborsNum { get; private set; }
    public int NeighborsInstantiated { get; private set; }

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

    private Vector3 CalculateShift(Vector3 curr, Vector2 corner) {
        return new Vector3(curr.x + corner.x, curr.y + corner.y, curr.z);
    }

    public Vector3 CurrentPosition(){
        return RoomObj.transform.position;
    }

    private void CalculateCorners(){
        Vector3 curr = CurrentPosition();
        RoomSettings rs = RoomObj.GetComponent<RoomSettings>();

        Vector3 calculatedTopLeft = CalculateShift(curr, rs.TopLeftCorner);
        Vector3 calculatedBottomRight = CalculateShift(curr, rs.BottomRightCorner);

        TopLeftCorner = new Vector3(
            Mathf.Min(calculatedTopLeft.x, calculatedBottomRight.x),
            Mathf.Max(calculatedTopLeft.y, calculatedBottomRight.y),
            curr.z
        );

        BottomRightCorner = new Vector3(
            Mathf.Max(calculatedTopLeft.x, calculatedBottomRight.x),
            Mathf.Min(calculatedTopLeft.y, calculatedBottomRight.y),
            curr.z
        );
    }

    public List<Door> GetFreeDoors(){
        Doorway dw = RoomObj.GetComponent<Doorway>();
        return dw.Doors.Where(door => !door.WasUsed).ToList();
    }

    public int DoorsNum(){
        Doorway dw = RoomObj.GetComponent<Doorway>();
        return dw.Doors.Count;
    }
}
