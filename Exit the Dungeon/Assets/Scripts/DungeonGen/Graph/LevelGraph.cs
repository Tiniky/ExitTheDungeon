using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelGraph_", menuName = "SZAKDOLGOZAT/Scriptable Objects/Dungeon/Level Graph")]

public class LevelGraph : ScriptableObject {
    public List<Room> Rooms = new List<Room>();
    public List<Connection> Connections = new List<Connection>();
    public List<GameObject> CorridorTemplates = new List<GameObject>();
    public List<GameObject> RoomTemplates = new List<GameObject>();
}
