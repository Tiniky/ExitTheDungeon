using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj{
    public InteractableType Type { get; private set; }
    public Vector2 Position { get; private set; }
    public GameObject Object { get; private set; }

    public InteractableObj(InteractableType type, Vector2 position, GameObject obj){
        Type = type;
        Position = position;
        Object = obj;
    }
}
