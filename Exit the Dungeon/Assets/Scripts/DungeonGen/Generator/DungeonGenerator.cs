using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DungeonGenerator {
    private static GameObject _dungeon = null;
    private static LevelGraph _currentGraph;
    private static List<InstantiatedRoom> _rooms = new List<InstantiatedRoom>();
    private static List<InstantiatedCorridor> _corridors = new List<InstantiatedCorridor>();
    private static List<GameObject> _corridorTemplates = new List<GameObject>();
    private static Dictionary<GameObject, int> _defaultTemplates = new Dictionary<GameObject, int>();
    private static bool _successfulBuild, _wasCalledFromEditor;
    private const int MaxAttempts = 100;
    private const int MaxGraphSelect = 10;
    private static List<Door> _triedDoors = new List<Door>();
    private static InstantiatedCorridor _lastCorridor;
    private static Door _lastDoor;
    private static Doorline3W _lastFromDoorLine, _lastToDoorLine;
    private static GameObject _wallLeft, _wallRight, _wallUp, _wallDown;
    private static Dungeon dungeon;

    public static bool GenerateDungeon(DungeonLevel lvl, bool WasCalledFromEditor = false){
        _wasCalledFromEditor = WasCalledFromEditor;
        int attempts = 0;
        ResetVariables();

        while(!_successfulBuild && attempts < MaxGraphSelect){
            if(attempts == MaxGraphSelect - 1){
                Debug.LogError("Dungeon Generation Failed: Too many attempts");
                return false;
            }

            _currentGraph = GetRandom(lvl.LevelGraphOptions);
            LoadTemplates();
            LoadWalls();
            attempts++;

            int buildAttempts = 0;
            while(!_successfulBuild && buildAttempts < MaxAttempts){
                if(buildAttempts == MaxAttempts - 1){
                    Debug.Log("Dungeon Generation Failed: Too many build attempts");
                    return false;
                }

                ClearDungeon();
                
                _successfulBuild = AttemptToBuildDungeon();
                buildAttempts++;
            }
        }

        if(_successfulBuild){
            PutUpWalls();
            dungeon = new Dungeon(_rooms, _corridors);
        }

        return _successfulBuild;
    }

    public static Dungeon GetDungeon(){
        return dungeon;
    }

    private static bool AttemptToBuildDungeon(){
        Queue<Room> roomQueue = new Queue<Room>();
        HashSet<Room> visitedRooms = new HashSet<Room>();
        Room startRoom = _currentGraph.Rooms.First();
        roomQueue.Enqueue(startRoom);

        bool NoOverlaps = ProcessQueue(roomQueue, visitedRooms);

        //Debug.Log("Expected Rooms: " + _currentGraph.Rooms.Count + " Rooms: " + _rooms.Count + " Expected Corridors: " + _currentGraph.Connections.Count + " Corridors: " + _corridors.Count);
        return roomQueue.Count == 0 && _rooms.Count == _currentGraph.Rooms.Count && NoOverlaps && _corridors.Count == _currentGraph.Connections.Count;
    }

    private static bool ProcessQueue(Queue<Room> roomQueue, HashSet<Room> visitedRooms){
        bool NoOverlaps = true;
        while(roomQueue.Count > 0){
            Room currentRoom = roomQueue.Dequeue();
            visitedRooms.Add(currentRoom);

            List<Room> neighbors = _currentGraph.Connections
            .Where(conn => conn.From == currentRoom || conn.To == currentRoom)
            .Select(conn => conn.From == currentRoom ? conn.To : conn.From)
            .ToList();
            int neighborCount = neighbors.Count;

            foreach(Room neighbor in neighbors){
                if(!visitedRooms.Contains(neighbor)){
                    roomQueue.Enqueue(neighbor);
                }
            }

            if(currentRoom.IsSpawn()){
                CreateRoom(currentRoom, neighborCount);
            } else{
                InstantiatedRoom neighborRoom = FindNeighborRoom(neighbors, visitedRooms);
                NoOverlaps = CanPlaceRoom(currentRoom, neighborRoom, neighborCount);
            
                if(!NoOverlaps){
                    break;
                }
            }
        }

        return NoOverlaps;
    }

    private static bool CanPlaceRoom(Room currentRoom, InstantiatedRoom neighborRoom, int neighborCount){
        if(neighborRoom == null){
            //Debug.Log("No neighbor room found");
            return false;
        }

        if(neighborRoom.NeighborsNum == neighborRoom.NeighborsInstantiated){
            //Debug.Log("All neighbors instantiated");
            return false;
        }

        InstantiatedCorridor corridor = CreateCorridor(neighborRoom);
        InstantiatedRoom newRoom = CreateRoom(currentRoom, neighborCount, corridor);

        if(corridor == null || newRoom == null){
            //Debug.Log("Corridor or Room not created");

            if(neighborRoom.GetFreeDoors().Where(d => !_triedDoors.Contains(d)).ToList().Count == 0){
                //Debug.Log("All doors tried");
                return false;
            } else{
                return CanPlaceRoom(currentRoom, neighborRoom, neighborCount);
            }
        }

        return true;
    }

    private static InstantiatedRoom CreateRoom(Room room, int neighbors, InstantiatedCorridor corridor = null){
        GameObject roomTemplate = SelectRoomTemplate(room);
        Vector3 roomPos = CalculateRoomPosition(roomTemplate, room, corridor);
        
        if(roomPos == Vector3.zero && corridor != null){
            //Debug.Log("Room position could not be calculated");

            if(_wasCalledFromEditor){
                GameObject.DestroyImmediate(_lastCorridor.CorridorObj);
            } else {
                GameObject.Destroy(_lastCorridor.CorridorObj);
            }

            return null;
        }

        GameObject roomObj = GameObject.Instantiate(roomTemplate, roomPos, Quaternion.identity, _dungeon.transform);
        InstantiatedRoom instantiatedRoom = new InstantiatedRoom(room, roomObj, neighbors);

        if(IsOverlapping(instantiatedRoom)){
            //Debug.Log("Room is overlapping");

            if(_wasCalledFromEditor){
                GameObject.DestroyImmediate(roomObj);
                GameObject.DestroyImmediate(_lastCorridor.CorridorObj);
            } else {
                GameObject.Destroy(roomObj);
                GameObject.Destroy(_lastCorridor.CorridorObj);
            }

            return null;
        }

        NameTheRoom(instantiatedRoom);
        _rooms.Add(instantiatedRoom);

        if(corridor != null){
            corridor.CloseUpDoors(instantiatedRoom);
            _corridors.Add(_lastCorridor);
            _lastDoor.WasUsed = true;
            //Debug.Log("Corridor FromDirection: " + corridor.FromDirection.ToString() + " ToDirection: " + corridor.ToDirection.ToString());

            corridor.From.AnotherNeighborDone(corridor.ToDirection, _lastFromDoorLine);
            instantiatedRoom.AnotherNeighborDone(corridor.FromDirection, _lastToDoorLine);
        } else {
            GameManager.CurrentRoom = instantiatedRoom;
        }

        return instantiatedRoom;
    }

    private static Vector3 CalculateRoomPosition(GameObject roomTemplate, Room room, InstantiatedCorridor corridor){
        Vector3 roomPos = Vector3.zero;

        if(!room.IsSpawn() && corridor != null){
            DoorDirection emptyEnd = corridor.ToDirection;
            DoorDirection roomExit = corridor.FromDirection;

            Doorway dw = roomTemplate.GetComponent<Doorway>();
            Door corriDoor = dw.Doors.FirstOrDefault(d => d.Direction == roomExit && !d.WasUsed);

            if(corriDoor == null){
                //Debug.Log("Corridor door not found, Can't place room");
                return roomPos;
            }

            Doorline3W doorPos = GetValidDoorPosition(corriDoor);
            _lastToDoorLine = doorPos;
            Vector3 startPos = doorPos.DoorStart;
            Vector3 shiftedPos = corridor.ShiftedToDoorPos();

            switch(emptyEnd){
                case DoorDirection.UP:
                    roomPos = new Vector3(
                        shiftedPos.x + startPos.x * (-1), 
                        shiftedPos.y + 1 + Mathf.Abs(startPos.y), 
                        0);
                    break;
                case DoorDirection.DOWN:
                    roomPos = new Vector3(
                        shiftedPos.x + startPos.x * (-1), 
                        shiftedPos.y - 1 - Mathf.Abs(startPos.y), 
                        0);
                    break;
                case DoorDirection.LEFT:
                    roomPos = new Vector3(
                        shiftedPos.x - 1 - Mathf.Abs(startPos.x), 
                        shiftedPos.y + startPos.y * (-1), 
                        0);
                    break;
                case DoorDirection.RIGHT:
                    roomPos = new Vector3(
                        shiftedPos.x + 1 + Mathf.Abs(startPos.x), 
                        shiftedPos.y + startPos.y * (-1), 
                        0);
                    break;
            }
        }

        return roomPos;
    }

    private static GameObject SelectRoomTemplate(Room room){
        return room.UniqueRoomTemplates.Count > 0 ? GetRandom(room.UniqueRoomTemplates) : GetLeastUsedTemplate();
    }

    private static GameObject GetLeastUsedTemplate() {
        var leastUsedTemplate = _defaultTemplates.OrderBy(t => _defaultTemplates.ContainsKey(t.Key) ? _defaultTemplates[t.Key] : 0).First().Key;
        _defaultTemplates[leastUsedTemplate]++;
        return leastUsedTemplate;
    }

    private static InstantiatedCorridor CreateCorridor(InstantiatedRoom neighborRoom){
        List<Door> freeDoors = neighborRoom.GetFreeDoors();

        if(freeDoors.Count == 0){
            //Debug.Log("No free doors found");
            return null;
        }

        List<Door> possibleDoors = freeDoors.Where(d => !_triedDoors.Contains(d)).ToList();
        if(possibleDoors.Count == 0){
            //Debug.Log("No possible doors found");
            return null;
        }
        Door door = GetRandom(possibleDoors);

        if(door == null){
            //Debug.Log("All doors tried");
            return null;
        }

        _lastDoor = door;
        _triedDoors.Add(door);
        DoorDirection dir = door.Direction;

        GameObject corridorTemplate = SelectCorridorTemplate(dir);
        Doorway dw = corridorTemplate.GetComponent<Doorway>();
        Vector3 corridorPos = CalculateCorridorPosition(neighborRoom.RoomObj, door, dw);
        
        GameObject corridorObj = GameObject.Instantiate(corridorTemplate, corridorPos, Quaternion.identity, _dungeon.transform);        
        InstantiatedCorridor corridor = new InstantiatedCorridor(corridorObj, neighborRoom, dir);
        _lastCorridor = corridor;
        NameTheCorridor(corridor);

        return corridor;
    }

    private static Vector3 CalculateCorridorPosition(GameObject neighborRoomObj, Door door, Doorway dw){
        Doorline3W doorPos = GetValidDoorPosition(door);
        _lastFromDoorLine = doorPos;
        Vector3 startPos = CalculateShiftedDoorPosition(neighborRoomObj.transform.position, doorPos.DoorStart);
        Door corriDoor = null;
        Vector3 corridorPos = Vector3.zero;

        switch(door.Direction){
            case DoorDirection.UP:
                corriDoor = dw.Doors.FirstOrDefault(d => d.Direction == DoorDirection.DOWN);
                if(corriDoor == null){
                    //Debug.Log("Corridor door not found");
                } else{
                    corridorPos = new Vector3(
                        startPos.x + corriDoor.PossibleStartPosition.x * (-1), 
                        startPos.y + 1 + Mathf.Abs(corriDoor.PossibleStartPosition.y), 
                        0);
                }
                break;
            case DoorDirection.DOWN:
                corriDoor = dw.Doors.FirstOrDefault(d => d.Direction == DoorDirection.UP);
                if(corriDoor == null){
                    //Debug.Log("Corridor door not found");
                } else{
                    corridorPos = new Vector3(
                        startPos.x + corriDoor.PossibleStartPosition.x * (-1), 
                        startPos.y - 1 - Mathf.Abs(corriDoor.PossibleStartPosition.y), 
                        0);
                }
                break;
            case DoorDirection.LEFT:
                corriDoor = dw.Doors.FirstOrDefault(d => d.Direction == DoorDirection.RIGHT);
                if(corriDoor == null){
                    //Debug.Log("Corridor door not found");
                } else{
                    corridorPos = new Vector3(
                        startPos.x - 1 - Mathf.Abs(corriDoor.PossibleStartPosition.x), 
                        startPos.y + corriDoor.PossibleStartPosition.y * (-1), 
                        0);
                }
                break;
            case DoorDirection.RIGHT:
                corriDoor = dw.Doors.FirstOrDefault(d => d.Direction == DoorDirection.LEFT);
                if(corriDoor == null){
                    //Debug.Log("Corridor door not found");
                } else{
                    corridorPos = new Vector3(
                        startPos.x + 1 + Mathf.Abs(corriDoor.PossibleStartPosition.x), 
                        startPos.y + corriDoor.PossibleStartPosition.y * (-1), 
                        0);
                }
                break;
        }

        return corridorPos;
    }

    private static Vector3 CalculateShiftedDoorPosition(Vector3 roomPos, Vector2 doorPos){
        return new Vector3(roomPos.x + doorPos.x, roomPos.y + doorPos.y, roomPos.z);
    }

    private static Doorline3W GetValidDoorPosition(Door door){
        Vector2 vec = Vector2.zero;

        if(door == null){
            //Debug.Log("Door is null o.o");
        }

        if(door.Direction == DoorDirection.UP || door.Direction == DoorDirection.DOWN){
            int minX = (int)Mathf.Min(door.PossibleStartPosition.x, door.PossibleEndPosition.x);
            int maxX = (int)Mathf.Max(door.PossibleStartPosition.x, door.PossibleEndPosition.x) - 2;
            int randomX = UnityEngine.Random.Range(minX, maxX + 1);
            vec = new Vector2(randomX, door.PossibleStartPosition.y);
        } else if(door.Direction == DoorDirection.LEFT || door.Direction == DoorDirection.RIGHT){
            int minY = (int)Mathf.Min(door.PossibleStartPosition.y, door.PossibleEndPosition.y) + 2;
            int maxY = (int)Mathf.Max(door.PossibleStartPosition.y, door.PossibleEndPosition.y);
            int randomY = UnityEngine.Random.Range(minY, maxY + 1);
            vec = new Vector2(door.PossibleStartPosition.x, randomY);
        }

        return new Doorline3W(vec, door.Direction);
    }

    private static void PutUpWalls(){
        ValidateRooms();

        foreach(var room in _rooms){
            Doorway doorway = room.RoomObj.GetComponent<Doorway>();
            foreach(var door in doorway.Doors){
                if(!door.WasUsed){
                    if(door.Direction == DoorDirection.UP || door.Direction == DoorDirection.DOWN){
                        //the corridor goes vertically but the door is horizontal
                        FillFromXtoX(room, door.PossibleStartPosition, door.PossibleEndPosition, door.Direction, true);
                    } else{
                        //the corridor goes horizontally but the door is vertical
                        FillFromYtoY(room, door.PossibleStartPosition, door.PossibleEndPosition, door.Direction, true);
                    }
                } else {
                    FillUpWalls(room, door, room.DoorPositionsUsed[door.Direction]);
                }
            }
        }
    }

    private static void FillUpWalls(InstantiatedRoom room, Door door, Doorline3W roomDoorline){
        if(door.Direction == DoorDirection.UP || door.Direction == DoorDirection.DOWN){
            FillUpHorizontalWalls(room, door, roomDoorline);
        } else{
            FillUpVerticalWalls(room, door, roomDoorline);
        }
    }

    private static void FillUpHorizontalWalls(InstantiatedRoom room, Door door, Doorline3W roomDoorline){
        FillFromXtoX(room, door.PossibleStartPosition, new Vector2(roomDoorline.DoorStart.x - 1, roomDoorline.DoorStart.y), door.Direction);
        FillFromXtoX(room, new Vector2(roomDoorline.DoorEnd.x + 1, roomDoorline.DoorEnd.y), door.PossibleEndPosition, door.Direction);
    }

    private static void FillUpVerticalWalls(InstantiatedRoom room, Door door, Doorline3W roomDoorline){
        FillFromYtoY(room, door.PossibleStartPosition, new Vector2(roomDoorline.DoorStart.x, roomDoorline.DoorStart.y + 1), door.Direction);
        FillFromYtoY(room, new Vector2(roomDoorline.DoorEnd.x, roomDoorline.DoorEnd.y - 1), door.PossibleEndPosition, door.Direction);
    }

    private static void FillFromXtoX(InstantiatedRoom room, Vector2 from, Vector2 to, DoorDirection dir, bool unusedDoor = false){
        float start = from.x;
        float end = to.x;

        if(start >= end){
            return;
        }

        GameObject wallHolder = room.RoomObj.transform.Find("Environment/WallObjects").gameObject;
        GameObject wallPrefab = WhichWallToUse(dir);
        Vector3 roomPos = room.CurrentPosition();

        if(unusedDoor){
            start -= 1;
            end += 1;
        }

        for(float i = start; i <= end; i++){
            Vector2 wallPos = new Vector2(i, from.y);
            Vector3 shiftedWallPos = CalculateShiftedWallPos(roomPos, wallPos, dir);
            GameObject wall = GameObject.Instantiate(wallPrefab, shiftedWallPos, Quaternion.identity, wallHolder.transform);
            NameTheWall(wall);
        }
    }

    private static void FillFromYtoY(InstantiatedRoom room, Vector2 from, Vector2 to, DoorDirection dir, bool unusedDoor = false){
        float start = from.y;
        float end = to.y;

        if(start <= end || start +1 <= end){
            return;
        }

        GameObject wallHolder = room.RoomObj.transform.Find("Environment/WallObjects").gameObject;
        GameObject wallPrefab = WhichWallToUse(dir);
        Vector3 roomPos = room.CurrentPosition();

        if(unusedDoor){
            start += 1;
            end -= 1;
        }

        for(float i = start; i >= end; i--){
            Vector2 wallPos = new Vector2(from.x, i);
            Vector3 shiftedWallPos = CalculateShiftedWallPos(roomPos, wallPos, dir);
            GameObject wall = GameObject.Instantiate(wallPrefab, shiftedWallPos, Quaternion.identity, wallHolder.transform);
            NameTheWall(wall);
        }
    }

    private static Vector3 CalculateShiftedWallPos(Vector3 roomPos, Vector2 wallPos, DoorDirection dir){
        Vector3 shiftedWallPos = Vector3.zero;

        switch(dir){
            case DoorDirection.UP:
                shiftedWallPos = new Vector3(roomPos.x + wallPos.x + 0.5f, roomPos.y + wallPos.y + 1.5f, 0);
                break;
            case DoorDirection.DOWN:
                shiftedWallPos = new Vector3(roomPos.x + wallPos.x + 0.5f, roomPos.y + wallPos.y - 0.5f, 0);
                break;
            case DoorDirection.LEFT:
                shiftedWallPos = new Vector3(roomPos.x + wallPos.x - 0.25f, roomPos.y + wallPos.y + 0.5f, 0);
                break;
            case DoorDirection.RIGHT:
                shiftedWallPos = new Vector3(roomPos.x + wallPos.x + 1.25f, roomPos.y + wallPos.y + 0.5f, 0);
                break;
        }

        return shiftedWallPos;
    }

    private static GameObject WhichWallToUse(DoorDirection dir){
        switch(dir){
            case DoorDirection.UP:
                return _wallUp;
            case DoorDirection.DOWN:
                return _wallDown;
            case DoorDirection.LEFT:
                return _wallLeft;
            case DoorDirection.RIGHT:
                return _wallRight;
        }

        return null;
    }

    private static bool ValidateRooms(){
        foreach(var room in _rooms){
            Doorway dw = room.RoomObj.GetComponent<Doorway>();
            List<Door> doors = dw.Doors.Where(d => d.WasUsed).ToList();

            bool allRoomsValid = doors.Count == room.NeighborsInstantiated && room.NeighborsInstantiated == room.DoorPositionsUsed.Count;
            if(!allRoomsValid){
                return false;
            }

            foreach(var door in doors){
                Doorline3W doorline = room.DoorPositionsUsed.ContainsKey(door.Direction) ? room.DoorPositionsUsed[door.Direction] : null;
                if(doorline == null){
                    return false;
                }
            }
        }

        return true;
    }

    private static GameObject SelectCorridorTemplate(DoorDirection dir){
        if (_corridorTemplates == null || _corridorTemplates.Count < 2) {
            //Debug.LogError("Corridor templates list is not properly initialized or has insufficient elements.");
            return null;
        }
        
        return dir == DoorDirection.UP || dir == DoorDirection.DOWN ? _corridorTemplates[0] : _corridorTemplates[1];
    }

    private static InstantiatedRoom FindNeighborRoom(List<Room> neighbors, HashSet<Room> visitedRooms){
        InstantiatedRoom neighborRoom = null;
        
        foreach(Room neighbor in neighbors){
            neighborRoom = _rooms.FirstOrDefault(r => r.Room == neighbor && visitedRooms.Contains(neighbor));
            if(neighborRoom != null){
                break;
            }
        }

        return neighborRoom;
    }

    private static bool IsOverlapping(InstantiatedRoom newRoom) {
        foreach (var room in _rooms) {
            if (IsBoundingBoxOverlapping(room, newRoom)) {
                return true;
            }
        }
        return false;
    }

    private static bool IsBoundingBoxOverlapping(InstantiatedRoom room1, InstantiatedRoom room2) {
        Vector3 min1 = room1.TopLeftCorner;
        Vector3 max1 = room1.BottomRightCorner;
        Vector3 min2 = room2.TopLeftCorner;
        Vector3 max2 = room2.BottomRightCorner;

        bool overlapX = min1.x <= max2.x && max1.x >= min2.x;
        bool overlapY = max1.y <= min2.y && min1.y >= max2.y;

        return overlapX && overlapY;
    }

    private static T GetRandom<T>(List<T> list) {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    private static void LoadTemplates(){
        _corridorTemplates.AddRange(_currentGraph.CorridorTemplates);
        foreach(GameObject template in _currentGraph.RoomTemplates){
            _defaultTemplates.Add(template, 0);
        }
    }

    private static void LoadWalls(){
        _wallLeft = Resources.Load<GameObject>("Prefabs/Dungeon/Rooms/WallTemplates/WALL_L");
        _wallRight = Resources.Load<GameObject>("Prefabs/Dungeon/Rooms/WallTemplates/WALL_R");
        _wallUp = Resources.Load<GameObject>("Prefabs/Dungeon/Rooms/WallTemplates/WALL_U");
        _wallDown = Resources.Load<GameObject>("Prefabs/Dungeon/Rooms/WallTemplates/WALL_D");
    }

    private static void ResetVariables(){
        ClearDungeon(true);
        _currentGraph = null;
        _corridorTemplates.Clear();
        _defaultTemplates.Clear();
        _successfulBuild = false;
        _dungeon = null;
    }

    private static void ClearDungeon(bool calledFromReset = false){
        if(!calledFromReset){
            if(_dungeon != null){
                if(_wasCalledFromEditor){
                    GameObject.DestroyImmediate(_dungeon);
                } else {
                    GameObject.Destroy(_dungeon);
                }
            }

            _dungeon = new GameObject("DUNGEON");
        }
        _rooms.Clear();
        _corridors.Clear();
        _triedDoors.Clear();
        _lastCorridor = null;
    }

    private static void NameTheRoom(InstantiatedRoom room){
        Vector3 pos = room.RoomObj.transform.position;
        room.RoomObj.name = room.Room.Type.ToString() 
            + "(" + pos.x.ToString() + ", " + pos.y.ToString() + ")_"
            + "(" + room.TopLeftCorner.x.ToString() + ", " + room.TopLeftCorner.y.ToString() + ")_"
            + "(" + room.BottomRightCorner.x.ToString() + ", " + room.BottomRightCorner.y.ToString() + ")";
    }
    
    private static void NameTheCorridor(InstantiatedCorridor corridor){
        Vector3 pos = corridor.CorridorObj.transform.position;
        corridor.CorridorObj.name = "CRD" + (corridor.FromDirection == DoorDirection.UP || corridor.FromDirection == DoorDirection.DOWN ? "_V_" : "_H_")
            + "(" + pos.x.ToString() + ", " + pos.y.ToString() + ")_"
            + "(" + corridor.TopLeftCorner.x.ToString() + ", " + corridor.TopLeftCorner.y.ToString() + ")_"
            + "(" + corridor.BottomRightCorner.x.ToString() + ", " + corridor.BottomRightCorner.y.ToString() + ")";
    }

    private static void NameTheWall(GameObject wall){
        Vector3 pos = wall.transform.position;
        wall.name = "WALL" + "(" + pos.x.ToString() + ", " + pos.y.ToString() + ")";
    }
}