using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatedRoom {
    public Room Room;
    public GameObject Template;
    public Vector3 TopLeftCorner, BottomRightCorner;
    public RoomSettings RoomSettings;
    public Doorway DoorWay;
    public SpawnPointHandler SpawnPointHandler;
    public int NeighboursNum, NeighboursInstantiated;

    public InstantiatedRoom(Room room, GameObject template, int neighbours){
        Room = room;
        Template = template;
        RoomSettings = template.GetComponent<RoomSettings>();
        DoorWay = template.GetComponent<Doorway>();
        SpawnPointHandler = template.GetComponent<SpawnPointHandler>();
        NeighboursNum = neighbours;
        TopLeftCorner = Vector3.zero;
        BottomRightCorner = Vector3.zero;
        NeighboursInstantiated = 0;
    }

    public void AnotherNeighbourDone(){
        NeighboursInstantiated++;
    }
}
