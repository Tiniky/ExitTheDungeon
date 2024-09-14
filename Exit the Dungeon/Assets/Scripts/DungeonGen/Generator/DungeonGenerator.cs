using System.Collections.Generic;
using UnityEngine;

public static class DungeonGenerator {
    private static GameObject _dungeon;
    private static LevelGraph _currentGraph;
    private static int _roomDB;
    private static bool _successfulBuild, _noOverlaps;
    private static List<InstantiatedRoom> _rooms;
    private static List<InstantiatedCorridor> _corridors;
    private static List<GameObject> _corridorTemplates;
    private static List<GameObject> _defaultTemplates;
    
    public static bool GenerateDungeon(DungeonLevel lvl){
        int buildAttempt = 0;
        ResetVariables();

        while(!_successfulBuild && buildAttempt < 10){
            buildAttempt++;

            _currentGraph = SelectGraph(lvl);
            LoadTemplates();
            int graphBuildAttempt = 0;

            while(!_successfulBuild && graphBuildAttempt < 100){
                graphBuildAttempt++;
                _successfulBuild = AttemptToBuildDungeon();
            }
        }

        return _successfulBuild;
    }

    private static void ResetVariables(){
        _dungeon = new GameObject("DUNGEON");
        _currentGraph = null;
        _roomDB = 0;
        _successfulBuild = false;
        _noOverlaps = false;
        _rooms = new List<InstantiatedRoom>();
        _corridors = new List<InstantiatedCorridor>();
        _corridorTemplates = new List<GameObject>();
        _defaultTemplates = new List<GameObject>();
    }

    private static T GetRandom<T>(List<T> list){
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    private static LevelGraph SelectGraph(DungeonLevel lvl){
        return GetRandom(lvl.LevelGraphOptions);
    }

    private static bool AttemptToBuildDungeon(){
        ProcessGraphWithDFS();

        if(_roomDB == _currentGraph.Rooms.Count){
            return true;
        }

        return false;
    }

    private static void LoadTemplates(){
        _corridorTemplates = _currentGraph.CorridorTemplates;
        _defaultTemplates = _currentGraph.RoomTemplates;
    }
    
    public static void ProcessGraphWithDFS(){
        if(_currentGraph.Rooms.Count == 0){
            return;
        }

        HashSet<Room> visitedRooms = new HashSet<Room>();
        Room startRoom = _currentGraph.Rooms[0];
        DFS(startRoom, visitedRooms);
    }

    private static void DFS(Room currentRoom, HashSet<Room> visitedRooms){
        visitedRooms.Add(currentRoom);
        _roomDB += 1;
        //Debug.Log("Visited Room: " + currentRoom.ID + " | Type: " + currentRoom.Type);

        HandleRoom(currentRoom);

        foreach(Connection connection in _currentGraph.Connections){
            Room next = null;

            if(connection.From == currentRoom){
                next = connection.To;
            } else if(connection.To == currentRoom){
                next = connection.From;
            }

            if(next != null && !visitedRooms.Contains(next)){
                DFS(next, visitedRooms);
            }
        }
    }

    private static void HandleRoom(Room room){
        GameObject template = GetTemplateOfRoom(room);

        if(room.Type == RoomType.SPAWN){
            GameManager.CurrentRoom = room;
            CreateDungeonRoomFromTemplate(template, room);
        }
    }

    private static GameObject GetTemplateOfRoom(Room room){
        GameObject template;

        if(room.UniqueRoomTemplates.Count > 0){
            template = GetRandom(room.UniqueRoomTemplates);
        } else {
            template = SelectRoomTemplate();
        }

        return template;
    }

    private static GameObject SelectCorridorTemplate(){
        return GetRandom(_corridorTemplates);
    }

    private static GameObject SelectRoomTemplate(){
        return GetRandom(_defaultTemplates);
    }

    private static void CreateDungeonRoomFromTemplate(GameObject template, Room room){
        Debug.Log("creating Room: " + room.Type);
        if(template != null){
            Vector3 position = GetRoomPosition(template, room);
            GameObject roomObj = GameObject.Instantiate(template, position, Quaternion.identity);
            roomObj.transform.SetParent(_dungeon.transform);

            int neighbours = CountNeighbours(room);
            Debug.Log("Room " + room.Type + " has Neighbours: " + neighbours);
            InstantiatedRoom instantiatedRoom = new InstantiatedRoom(room, roomObj, neighbours);
            _rooms.Add(instantiatedRoom);
        }
    }

    private static Vector3 GetRoomPosition(GameObject template, Room room){
        if(room.Type == RoomType.SPAWN){
            return Vector3.zero;
        }

        return Vector3.zero;
    }

    private static int CountNeighbours(Room room){
        int num = 0;

        foreach(Connection connection in _currentGraph.Connections){
            if(connection.From == room || connection.To == room){
                num++;
            }
        }

        return num;
    }
}
