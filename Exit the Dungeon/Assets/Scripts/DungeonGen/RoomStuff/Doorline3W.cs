using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorline3W {
    public Vector3 DoorStart, DoorMiddle, DoorEnd;
    public DoorDirection Direction;

    public Doorline3W(Vector3 start, DoorDirection dir){
        DoorStart = start;
        if(dir == DoorDirection.LEFT || dir == DoorDirection.RIGHT){
            DoorMiddle = new Vector3(start.x, start.y + 1, 0);
            DoorEnd = new Vector3(start.x, start.y - 2, 0);
        } else if(dir == DoorDirection.UP || dir == DoorDirection.DOWN){
            DoorMiddle = new Vector3(start.x + 1, start.y, 0);
            DoorEnd = new Vector3(start.x + 2, start.y, 0);
        }
    }
}
