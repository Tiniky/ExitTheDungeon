using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour {
    private GameObject _player;

    void Start() {
        _player = GameManager.PlayerObj();
    }

    private void LateUpdate() {
        transform.position = _player.transform.position + new Vector3(0,0,-10);
    }
}
