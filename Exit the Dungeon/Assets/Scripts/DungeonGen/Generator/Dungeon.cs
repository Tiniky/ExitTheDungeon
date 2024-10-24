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
        PlayerSpawnPoint psp = RoomObj.GetComponent<PlayerSpawnPoint>();
        return new Vector3(psp.SpawnPoint.x, psp.SpawnPoint.y, 0);
    }

    public InstantiatedRoom GetRoom(GameObject roomObj){
        return Rooms.FirstOrDefault(r => r.RoomObj == roomObj);
    }

    public InstantiatedCorridor GetCorridor(GameObject corrObj){
        return Corridors.FirstOrDefault(c => c.CorridorObj == corrObj);
    }

    public void TurnOffInteractableTiles(){
        foreach(InstantiatedRoom room in Rooms){
            Transform tileHolderTransform = room.RoomObj.transform.Find("Environment/InteractableTiles");
            if(tileHolderTransform != null){
                GameObject tileHolder = tileHolderTransform.gameObject;
                tileHolder.SetActive(false);
                RoomTiles.Add(room, tileHolder);
            }
        }
    }

    public void SetVisibilityOfTiles(bool visible){
        GameObject tileHolder = RoomTiles[GameManager.CurrentRoom];
        tileHolder.SetActive(visible);
    }

    public Dictionary<InstantiatedRoom, Vector2> GetSpawnPointsOfPartyMember(){
        Dictionary<InstantiatedRoom, Vector2> spawnPoints = new Dictionary<InstantiatedRoom, Vector2>();

        foreach(InstantiatedRoom room in Rooms){
            if(room.Room.Type == RoomType.PRISON){
                PartyMemberSpawnPoint pmsp = room.RoomObj.GetComponent<PartyMemberSpawnPoint>();
                spawnPoints.Add(room, pmsp.SpawnPoint);
            }
        }

        return spawnPoints;
    }

    public Dictionary<InstantiatedRoom, List<Vector2>> GetSpawnPointsOfEnemies(){
        Dictionary<InstantiatedRoom, List<Vector2>> spawnPoints = new Dictionary<InstantiatedRoom, List<Vector2>>();

        foreach(InstantiatedRoom room in Rooms){
            if(room.Room.Type == RoomType.COMBAT || room.Room.Type == RoomType.PRISON){
                List<Vector2> points = new List<Vector2>();
                EnemySpawnPoint esp = room.RoomObj.GetComponent<EnemySpawnPoint>();
                
                foreach(Vector2 sp in esp.SpawnPoints){
                    points.Add(sp);
                }

                spawnPoints.Add(room, points);
            }
        }

        return spawnPoints;
    }

    public List<InstantiatedRoom> DoorNeeded(){
        List<InstantiatedRoom> rooms = new List<InstantiatedRoom>();

        foreach(InstantiatedRoom room in Rooms){
            if(room.Room.Type == RoomType.COMBAT || room.Room.Type == RoomType.GEM || room.Room.Type == RoomType.PRISON || room.Room.Type == RoomType.BOSS){
                rooms.Add(room);
            }
        }

        return rooms;
    }

    public List<InstantiatedRoom> InteractableNeeded(){
        List<InstantiatedRoom> rooms = new List<InstantiatedRoom>();

        foreach(InstantiatedRoom room in Rooms){
            if(room.Room.Type == RoomType.GEM || room.Room.Type == RoomType.PRISON || room.Room.Type == RoomType.TREASURE){
                rooms.Add(room);
            }
        }

        return rooms;
    }
}
