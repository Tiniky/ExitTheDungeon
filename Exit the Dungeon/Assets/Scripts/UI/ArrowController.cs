using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {
    private Vector3 _startPos;

    void Start() {
        BattleManager.InitializeArrow(gameObject);
        _startPos = gameObject.transform.position;
    }

    public void Reset(){
        gameObject.transform.position = _startPos;
    }
}
