using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DungeonGenerator {
    private static GameObject _dungeon;
    private static LevelGraph _currentGraph;
    private static List<InstantiatedRoom> _rooms = new List<InstantiatedRoom>();
    private static List<InstantiatedCorridor> _corridors;
    private static List<GameObject> _corridorTemplates;
    private static List<GameObject> _defaultTemplates;
    private static bool _successfulBuild, _noOverlaps, _wasCalledFromEditor;

    public static bool GenerateDungeon(DungeonLevel lvl, bool WasCalledFromEditor = false){
        int buildAttempt = 0;
        _wasCalledFromEditor = WasCalledFromEditor;
        ResetVariables();

        while(!_successfulBuild && buildAttempt < 10){
            buildAttempt++;
            _currentGraph = SelectRandomGraph(lvl);
            LoadTemplates();
            int graphBuildAttempt = 0;

            while(!_successfulBuild && graphBuildAttempt < 1000){
                ClearDungeon();
                graphBuildAttempt++;
                _successfulBuild = AttemptToBuildDungeon();
            }
        }

        return _successfulBuild;
    }

    private static void ResetVariables(){
        if (_dungeon != null){
            if(_wasCalledFromEditor){
                GameObject.DestroyImmediate(_dungeon);
            } else {
                GameObject.Destroy(_dungeon);
            }
            
        }

        _dungeon = new GameObject("DUNGEON");
        _currentGraph = null;
        _successfulBuild = false;
        _noOverlaps = true;
        _rooms = new List<InstantiatedRoom>();
        _corridors = new List<InstantiatedCorridor>();
        _corridorTemplates = new List<GameObject>();
        _defaultTemplates = new List<GameObject>();
        Debug.Log("PG setup done");
    }

    private static T GetRandom<T>(List<T> list){
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    private static LevelGraph SelectRandomGraph(DungeonLevel lvl){
        return GetRandom<LevelGraph>(lvl.LevelGraphOptions);
    }

    private static void LoadTemplates(){
        _corridorTemplates = _currentGraph.CorridorTemplates;
        _defaultTemplates = _currentGraph.RoomTemplates;
        Debug.Log("PG templates loaded");
    }

    private static void ClearDungeon(){
        if (_dungeon != null){
            if(_wasCalledFromEditor){
                GameObject.DestroyImmediate(_dungeon);
            } else {
                GameObject.Destroy(_dungeon);
            }
            
        }

        _dungeon = new GameObject("DUNGEON");
        _rooms.Clear();
        _corridors.Clear();
    }

    private static bool AttemptToBuildDungeon(){
        Queue<Room> roomQueue = new Queue<Room>();
        Room startRoom = _currentGraph.Rooms[0];
        HashSet<Room> visitedRooms = new HashSet<Room>();

        if(startRoom != null){
            roomQueue.Enqueue(startRoom);
        } else{
            Debug.Log("PG no spawn room o.o");
            return false;
        }

        _noOverlaps = ProcessQueue(roomQueue, visitedRooms);

        if(roomQueue.Count == 0 && visitedRooms.Count == _currentGraph.Rooms.Count && _noOverlaps){
            return true;
        } else {
            return false;
        }
    }

    private static bool ProcessQueue(Queue<Room> roomQueue, HashSet<Room> visitedRooms){
        bool noOverlap = true;

        while(roomQueue.Count > 0 && noOverlap){
            Room currentRoom = roomQueue.Dequeue();
            visitedRooms.Add(currentRoom);
            List<Room> neighbors = GetNeighborsOf(currentRoom, visitedRooms);
            int neighborsNum = neighbors.Count;
            
            foreach(Room room in neighbors){
                roomQueue.Enqueue(room);
            }

            if(currentRoom.Type == RoomType.SPAWN){
                GameManager.CurrentRoom = currentRoom;
                CreateRoomFromTemplate(currentRoom, neighborsNum);
            } else {
                InstantiatedRoom neighbor = FindNeighbor(currentRoom);
                noOverlap = CanPlaceRoom(currentRoom, neighbor, neighborsNum);

                if(!noOverlap){
                    break;
                }
            }
        }

        return noOverlap;
    }

    private static List<Room> GetNeighborsOf(Room room, HashSet<Room> visitedRooms){
        List<Room> rooms = new List<Room>();

        foreach(Connection connection in _currentGraph.Connections){
            Room next = null;

            if(connection.From == room){
                next = connection.To;
            } else if(connection.To == room){
                next = connection.From;
            }

            if(next != null && !visitedRooms.Contains(next)){
                rooms.Add(next);
            }
        }

        return rooms;
    }

    private static GameObject GetTemplateOf(Room room){
        GameObject template;

        if(room.UniqueRoomTemplates.Count > 0){
            template = GetRandom<GameObject>(room.UniqueRoomTemplates);
        } else {
            template = GetRandom<GameObject>(_defaultTemplates);
        }

        return template;
    }

    private static InstantiatedRoom CreateRoomFromTemplate(Room room, int neighbors, InstantiatedCorridor crd = null){
        GameObject template = GetTemplateOf(room);
        Vector3 position = CalculateRoomPosition(template, room, crd);

        if(position == Vector3.zero && crd != null){
            Debug.Log("PG wrong template selected");
            return null;
        }
        
        GameObject roomObj = GameObject.Instantiate(template, position, Quaternion.identity);
        NameTheRoom(roomObj, room.Type);
        roomObj.transform.SetParent(_dungeon.transform);
        
        InstantiatedRoom instantiatedRoom = new InstantiatedRoom(room, roomObj, neighbors);
        _rooms.Add(instantiatedRoom);

        if(crd != null){
            crd.CloseUpDoors(instantiatedRoom);
        }

        return instantiatedRoom;
    }

    private static Vector3 CalculateRoomPosition(GameObject template, Room room, InstantiatedCorridor crd){
        Vector3 position = Vector3.zero;

        if(room.Type != RoomType.SPAWN && crd != null){
            DoorDirection emptyEnd = crd.ToDirection;
            DoorDirection roomExit = crd.FromDirection;
            Doorway doorWay = template.GetComponent<Doorway>();
            Door selectedDoor = doorWay.Doors.FirstOrDefault(door => door.Direction == roomExit && !door.WasUsed);
            
            if(selectedDoor == null){
                Debug.Log("PG no door on room template");
                return position;
            } else {
                Doorline3W doorPos = GetValidDoorPosition(selectedDoor);
                Vector3 start = doorPos.DoorStart;

                Vector3 shiftedPos = crd.ShiftedToDoorPos();

                switch(emptyEnd){
                    case DoorDirection.UP:
                        position.x = shiftedPos.x + start.x * (-1);
                        position.y = shiftedPos.y + 1 + Mathf.Abs(start.y);
                        break;
                    case DoorDirection.RIGHT:
                        position.x = shiftedPos.x + 1 + Mathf.Abs(start.x);
                        position.y = shiftedPos.y + start.y * (-1);
                        break;
                    case DoorDirection.DOWN:
                        position.x = shiftedPos.x + start.x * (-1);
                        position.y = shiftedPos.y - 1 - Mathf.Abs(start.y);
                        break;
                    case DoorDirection.LEFT:
                        position.x = shiftedPos.x - 1 - Mathf.Abs(start.x);
                        position.y = shiftedPos.y + start.y *(-1);
                        break;
                    default:
                        break;
                }
            }
        }

        return position;
        
    }

    private static InstantiatedRoom FindNeighbor(Room room){
        List<InstantiatedRoom> neighborOptions = new List<InstantiatedRoom>();
        List<Connection> filteredConnections = _currentGraph.Connections.Where(conn => room == conn.From || room == conn.To).ToList();

        foreach(Connection connection in filteredConnections){
            Room possibleNeighbor = null;

            if(connection.From == room){
                possibleNeighbor = connection.To;
            } else if(connection.To == room){
                possibleNeighbor = connection.From;
            }

            InstantiatedRoom ir = _rooms.FirstOrDefault(r => r.Room == possibleNeighbor);
            neighborOptions.Add(ir);
        }

        return GetRandom<InstantiatedRoom>(neighborOptions);
    }

    private static bool CanPlaceRoom(Room room, InstantiatedRoom neighbor, int neighborsNum){
        if(neighbor == null){
            return false;
        }
        
        if(neighbor.NeighborsNum == neighbor.NeighborsInstantiated){
            Debug.Log("PG all the neighbors instantiated already");
            return false;
        }

        InstantiatedCorridor crd = CreateCorridor(neighbor);
        InstantiatedRoom instRoom = CreateRoomFromTemplate(room, neighborsNum, crd);

        if(crd == null || instRoom == null){
            return false;
        }

        return IsNotOverlapping();
    }

    private static InstantiatedCorridor CreateCorridor(InstantiatedRoom room){
        Doorway dw = room.RoomObj.GetComponent<Doorway>();
        List<Door> doors = room.GetFreeDoors();
        
        if(doors.Count == 0){
            Debug.Log("PG no door on roomObj o.o");
            return null;
        }

        Door door = GetRandom<Door>(doors);
        door.WasUsed = true;

        DoorDirection dir = door.Direction;
        GameObject corridorTemplate = SelectCorridorTemplate(dir);
        Doorway corridorDoors = corridorTemplate.GetComponent<Doorway>();
        Vector3 corridorPosition = CalculateCorridorPosition(room.RoomObj, door, corridorDoors);

        GameObject corridorObj = GameObject.Instantiate(corridorTemplate, corridorPosition, Quaternion.identity);
        NameTheCorridor(corridorObj,dir);
        corridorObj.transform.SetParent(_dungeon.transform);
        
        InstantiatedCorridor corridor = new InstantiatedCorridor(corridorObj, room, dir);
        _corridors.Add(corridor);

        return corridor;
    }

    private static GameObject SelectCorridorTemplate(DoorDirection direction){
        if(direction == DoorDirection.UP || direction == DoorDirection.DOWN){
            return _corridorTemplates[0];
        } else {
            return _corridorTemplates[1];
        }
    }

    private static Vector3 CalculateCorridorPosition(GameObject currentRoom, Door door, Doorway corridor){
        Doorline3W doorPos = GetValidDoorPosition(door);
        Vector3 start = CalculateShiftedStartPos(currentRoom, doorPos.DoorStart);
        Door corridorDoor = null;
        Vector3 position = Vector3.zero;

        switch(door.Direction){
            case DoorDirection.UP:
                corridorDoor = corridor.Doors.Single(tempDoor => tempDoor.Direction == DoorDirection.DOWN);
                if(corridorDoor != null){
                    position.x = start.x + corridorDoor.PossibleStartPosition.x * (-1);
                    position.y = start.y + 1 + Mathf.Abs(corridorDoor.PossibleStartPosition.y);
                }
                break;
            case DoorDirection.RIGHT:
                corridorDoor = corridor.Doors.Single(tempDoor => tempDoor.Direction == DoorDirection.LEFT);
                if(corridorDoor != null){
                    position.x = start.x + 1 + Mathf.Abs(corridorDoor.PossibleStartPosition.x);
                    position.y = start.y + corridorDoor.PossibleStartPosition.y  * (-1);
                }
                break;
            case DoorDirection.DOWN:
                corridorDoor = corridor.Doors.Single(tempDoor => tempDoor.Direction == DoorDirection.UP);
                if(corridorDoor != null){
                    position.x = start.x + corridorDoor.PossibleStartPosition.x * (-1);
                    position.y = start.y - 1 - Mathf.Abs(corridorDoor.PossibleStartPosition.y);
                }
                break;
            case DoorDirection.LEFT:
                corridorDoor = corridor.Doors.Single(tempDoor => tempDoor.Direction == DoorDirection.RIGHT);
                if(corridorDoor != null){
                    position.x = start.x - 1 - Mathf.Abs(corridorDoor.PossibleStartPosition.x);
                    position.y = start.y + corridorDoor.PossibleStartPosition.y * (-1);
                }
                break;
        }

        return position;
    }

    private static Doorline3W GetValidDoorPosition(Door door){
        Vector2 vec = Vector2.zero;

        if(door == null){
            Debug.Log("PG no door o.o");
        }

        if(door.PossibleStartPosition.y == door.PossibleEndPosition.y){
            int minX = (int)Mathf.Min(door.PossibleStartPosition.x, door.PossibleEndPosition.x);
            int maxX = (int)Mathf.Max(door.PossibleStartPosition.x, door.PossibleEndPosition.x) - 2;
            int randomX = UnityEngine.Random.Range(minX, maxX + 1);

            vec = new Vector2(randomX, door.PossibleStartPosition.y);

        } else if(door.PossibleStartPosition.x == door.PossibleEndPosition.x){
            int minY = (int)Mathf.Min(door.PossibleStartPosition.y, door.PossibleEndPosition.y) + 2;
            int maxY = (int)Mathf.Max(door.PossibleStartPosition.y, door.PossibleEndPosition.y);
            int randomY = UnityEngine.Random.Range(minY, maxY + 1);
            
            vec = new Vector2(door.PossibleStartPosition.x, randomY);
        }
        
        return new Doorline3W(vec, door.Direction);
    }

    private static Vector3 CalculateShiftedStartPos(GameObject obj, Vector3 pos){
        Vector3 shiftedPosition = Vector3.zero;
        shiftedPosition.x = obj.transform.position.x + pos.x;
        shiftedPosition.y = obj.transform.position.y + pos.y;
        return shiftedPosition;
    }

    private static bool IsNotOverlapping(){
        //needs implementation
        return true;
    }

    private static void NameTheRoom(GameObject obj, RoomType type){
        Vector3 pos = obj.transform.position;
        obj.name = type.ToString() + "(" + pos.x.ToString() + ", " + pos.y.ToString() + ")";
    }
    
    private static void NameTheCorridor(GameObject obj, DoorDirection dir){
        Vector3 pos = obj.transform.position;
        obj.name = "CRD" + (dir == DoorDirection.UP || dir == DoorDirection.DOWN ? "_V_" : "_H_") + "(" + pos.x.ToString() + ", " + pos.y.ToString() + ")";
    }
}