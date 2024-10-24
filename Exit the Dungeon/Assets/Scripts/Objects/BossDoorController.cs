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
            LogManager.AddMessage("The door disappears. Go ahead, what can go wrong?");
        } else{
            LogManager.AddMessage("The door is sealed shut. You might need to collect all the gems to open it.");
        }
    }
}
