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
        if(wasOpened == false){
            if(GameManager.HasKey){
                wasOpened = true;
                LogManager.AddMessage("The chest is locked.. Never mind it seems like you got the key.");
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
                }
            } else { 
                LogManager.AddMessage("The chest is locked. Seems like you need the key to open it.");
            }
        }
    }
}
