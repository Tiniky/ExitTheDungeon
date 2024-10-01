using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : ScriptableObject {
    public string ID;
    public string Name;
    public RoomType Type;
    public Vector2 Position; //editor
    public bool IsSelected, IsFocused; //editor
    public List<GameObject> UniqueRoomTemplates;

    public void Initialize(RoomType roomType = RoomType.NONE){
        Type = roomType;
        UniqueRoomTemplates = new List<GameObject>();
        ID = System.Guid.NewGuid().ToString();
        IsSelected = false;
        IsFocused = false;
        this.name = "Room";

        SetName();
    }

    public void SetRoomType(RoomType roomType){
        Type = roomType;
        SetName();
    }

    public void SetName(){
        if(Type != RoomType.NONE){
            Name = Type.ToString();
        } else {
            Name = "UNDEFINED";
        }
    }

    public string GetDisplayName() {
        return Name;
    }

    public bool IsSpawn(){
        return Type == RoomType.SPAWN;
    }

    public override bool Equals(object obj){
        if(obj == null || GetType() != obj.GetType()){
            return false;
        }

        Room room = (Room)obj;
        return ID == room.ID;
    }

    public override int GetHashCode() {
        return ID.GetHashCode();
    }
}
