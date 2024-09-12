using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Interactable {
    public Animator animator;
    public GameObject scroll;
    public bool wasOpened;

    void Start() {
        wasOpened = false;
        isInRange = false;
        isStillInteractable = true;
    }

    void Update() {
        UpdateTrigger();
    }

    protected override void Interact() {
        OpenChest();
    }

    void OpenChest() {
        if(wasOpened == false){
            wasOpened = true;
            isStillInteractable = false;
            TextUIManager.UpdateInteractableText(false);
            animator.SetBool("isOpen", true);
            scroll.GetComponent<SpriteRenderer>().enabled = true;
            scroll.GetComponent<ScrollController>().animator.SetBool("wasReleased", true);
            scroll.GetComponent<ScrollController>().AwakenCutscene();
            Debug.Log("open");
        }
    }
}
