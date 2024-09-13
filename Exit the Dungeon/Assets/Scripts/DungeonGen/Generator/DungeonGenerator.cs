using System.Collections.Generic;
using UnityEngine;

public static class DungeonGenerator {
    private static int _roomDB;
    private static bool _successfulBuild;

    public static void DepthFirstSearch(LevelGraph graph){
        if(graph.Rooms.Count == 0){
            return;
        }

        _roomDB = 0;
        _successfulBuild = false;
        HashSet<Room> visitedRooms = new HashSet<Room>();
        Room startRoom = graph.Rooms[0];
        DFS(startRoom, visitedRooms, graph);
    }

    private static void DFS(Room currentRoom, HashSet<Room> visitedRooms, LevelGraph graph){
        
        visitedRooms.Add(currentRoom);
        _roomDB += 1;
        Debug.Log("Visited Room: " + currentRoom.ID + " | Type: " + currentRoom.Type);

        foreach(Connection connection in graph.Connections){
            Room next = null;

            if(connection.From == currentRoom){
                next = connection.To;
            } else if(connection.To == currentRoom){
                next = connection.From;
            }

            if(next != null && !visitedRooms.Contains(next)){
                DFS(next, visitedRooms, graph);
            }
        }
    }

    public static bool GenerateDungeon(DungeonLevel lvl){
        int randomIndex = Random.Range(0, lvl.LevelGraphOptions.Count);
        LevelGraph graph = lvl.LevelGraphOptions[randomIndex];
        DepthFirstSearch(graph);

        if(_roomDB == graph.Rooms.Count){
            _successfulBuild = true;
        }

        return _successfulBuild;
    }
}
