using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Interactable {
    public Animator animator;
    public ChestType Type;
    public GameObject Content;
    public bool wasOpened;

    void Start(){
        wasOpened = false;
        isInRange = false;
        isStillInteractable = true;
    }

    void Update(){
        UpdateTrigger();
    }

    protected override void Interact(){
        OpenChest();
    }

    void OpenChest(){
        //for now it can't be opened
        if(wasOpened == false){
            Debug.Log("ChestController - chest is locked");

            /*wasOpened = true;
            isStillInteractable = false;
            TextUIManager.UpdateInteractableText(false);
            animator.SetBool("isOpen", true);
            Content.GetComponent<SpriteRenderer>().enabled = true;

            if(Type == ChestType.ITEM){
                Content.GetComponent<ItemController>().animator.SetBool("wasReleased", true);
                Content.GetComponent<ItemController>().AwakenCutscene();
                Debug.Log("open");
            } else if(Type == ChestType.SCROLL){
                Content.GetComponent<ScrollController>().animator.SetBool("wasReleased", true);
                Content.GetComponent<ScrollController>().AwakenCutscene(gameObject);
                Debug.Log("open");
            }*/
        }
    }
}
