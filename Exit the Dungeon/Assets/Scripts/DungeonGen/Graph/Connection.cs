using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : ScriptableObject {
    public Room From;
    public Room To;
    public bool IsSelected;

    public void Initialize(Room from, Room to){
        From = from;
        To = to;
        IsSelected = false;
        this.name = "Connection";
    }
}
