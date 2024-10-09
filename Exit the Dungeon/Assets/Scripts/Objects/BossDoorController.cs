using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorController : Interactable {

    void Start(){
        interactKey = KeyCode.F;
        isInRange = false;
        isStillInteractable = true;
    }

    void Update(){
        UpdateTrigger();
    }

    protected override void Interact() {
        InteractWithDoor();
    }

    private void InteractWithDoor(){
        if(GameManager.Gem == 4){
            GameObject doorHolder = gameObject.transform.parent.gameObject;
            doorHolder.SetActive(false);
            Debug.Log("BDC - Door opened");
        }

        Debug.Log("BDC - Gem count is not enough, gems: " + GameManager.Gem);
    }
}
