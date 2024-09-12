using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
    public bool isInRange, isStillInteractable;
    public KeyCode interactKey;
    public UnityEvent interact;

    protected void UpdateTrigger() {
        if(isInRange && Input.GetKeyDown(interactKey)){
            Interact();
        }
    }

    protected virtual void Interact() {
        interact.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(isStillInteractable){
            if(collision.gameObject.CompareTag("Player")){
                isInRange = true;
                TextUIManager.UpdateInteractableText(isInRange);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(isStillInteractable){
            if(collision.gameObject.CompareTag("Player")){
                isInRange = false;
                TextUIManager.UpdateInteractableText(isInRange);
            }
        }
    }
}
