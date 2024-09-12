using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidController : MonoBehaviour {
    public bool cutsceneON;

    void Start() {
        cutsceneON = false;
        gameObject.SetActive(false);
    }

    void Update() {}

    public void StartCutscene(){
        cutsceneON = true;
        gameObject.SetActive(true);
        UIManager.UIVisibility(false);
        TextUIManager.UpdateInteractableText(false);
    }

    public void EndCutscene(){
        cutsceneON = false;
        gameObject.SetActive(false);
        UIManager.UIVisibility(true);
        TextUIManager.UpdateInteractableText(true);
    }
}
