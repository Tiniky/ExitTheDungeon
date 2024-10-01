using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door {

    //ID
    public Vector2 PossibleStartPosition;
    public Vector2 PossibleEndPosition;
    public DoorDirection Direction;
    public GameObject DoorPrefab;
    private const int _doorLength = 3;
    public bool WasUsed = false;
}