using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatedCorridor {
    public InstantiatedRoom From, To;
    public GameObject Template;
    public Vector3 TopLeftCorner, BottomRightCorner;

    public InstantiatedCorridor(GameObject template, InstantiatedRoom room){
        From = room;
        Template = template;
        TopLeftCorner = Vector3.zero;
        BottomRightCorner = Vector3.zero;
    }
}
