using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {
    private bool _wasInit;

    void Start(){
        _wasInit = false;
    }

    void Update(){
        //and if fire is in combat room
        if(GameManager.InFight() && !_wasInit){
            _wasInit = true;

            TileManager.instance.IgniteTile(transform.position);
        }
    }
}
