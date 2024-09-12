using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : Interactable {
    public Animator animator;
    public bool wasPickedUp;

    void Start() {
        wasPickedUp = false;
        isInRange = false;
        isStillInteractable = true;
    }

    void Update() {
        UpdateTrigger();
    }

    protected override void Interact() {
        if(!wasPickedUp){
            PickUpGem();
        }
    }

    void PickUpGem() {
        wasPickedUp = true;
        isStillInteractable = false;
        TextUIManager.UpdateInteractableText(false);
        Destroy(gameObject);
        GemUIManager.Add();
    }
}
