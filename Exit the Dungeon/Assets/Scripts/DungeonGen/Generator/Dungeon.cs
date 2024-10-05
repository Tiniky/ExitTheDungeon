using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon {
    public List<InstantiatedRoom> Rooms {get; private set;}
    public List<InstantiatedCorridor> Corridors {get; private set;}
    public Dictionary<InstantiatedRoom, GameObject> RoomTiles {get; private set;}

    public Dungeon(List<InstantiatedRoom> rooms, List<InstantiatedCorridor> corridors){
        Rooms = new List<InstantiatedRoom>();
        Corridors = new List<InstantiatedCorridor>();
        RoomTiles = new Dictionary<InstantiatedRoom, GameObject>();
        
        foreach(InstantiatedRoom room in rooms){
            Rooms.Add(room);
        }

        foreach(InstantiatedCorridor corridor in corridors){
            Corridors.Add(corridor);
        }
    }

    public Vector3 GetSpawnPointOfPlayer(){
        GameObject RoomObj = Rooms.FirstOrDefault(r => r.Room.IsSpawn()).RoomObj;
        SpawnPointHandler sph = RoomObj.GetComponent<SpawnPointHandler>();
        return new Vector3(sph.PlayerSpawnpoint.x, sph.PlayerSpawnpoint.y, 0);
    }

    public InstantiatedRoom GetRoom(GameObject roomObj){
        return Rooms.FirstOrDefault(r => r.RoomObj == roomObj);
    }

    public InstantiatedCorridor GetCorridor(GameObject corrObj){
        return Corridors.FirstOrDefault(c => c.CorridorObj == corrObj);
    }

    public void TurnOffInteractableTiles(){
        foreach(InstantiatedRoom room in Rooms){
            GameObject tileHolder = room.RoomObj.transform.Find("Environment/InteractableTiles").gameObject;
            tileHolder.SetActive(false);
            RoomTiles.Add(room, tileHolder);
        }
    }
}
