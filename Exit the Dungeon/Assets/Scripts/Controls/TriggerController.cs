using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {
    public List<GameObject> doors;
    public GameObject obstacle;
    public bool isResponsibleForObstacles, shouldCheckForGem;

    void Start() {
        if(isResponsibleForObstacles){
                foreach(GameObject door in doors){
                door.SetActive(false);
            }
        }
    }

    void Update(){}

    private void OnTriggerEnter2D(Collider2D collision){
        string tag = "";
        if(GameManager.WasSomeoneSaved()){
            tag = "Ally";
        } else {
            tag = "Player";
        }
        
        if(collision.gameObject.CompareTag(tag)){
            if(isResponsibleForObstacles){
                foreach(GameObject door in doors){
                    door.SetActive(true);
                }
            }

            if(shouldCheckForGem && GameManager.Gem > 0){
                obstacle.SetActive(false);
            }
        }
    }
}
