using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable {
    
    public Animator doorAnimator;
    public bool isOpen;
    public PartyMemberBehaviour hostage;

    void Start() {
        isOpen = false;
        isInRange = false;
        isStillInteractable = false;
    }

    void Update() {
        UpdateTrigger();
    }

    protected override void Interact() {
        DoorState();
    }

    private void DoorState(){
        if(isOpen){
            Debug.Log("the door is open");
        } else {
            Debug.Log("the door is closed");
        }
    }

    public void Open() {
        doorAnimator.SetBool("LeverWasPulled", true);

        isOpen = true;

        if(hostage != null){
            hostage.Escaped();
        }
    }
}
