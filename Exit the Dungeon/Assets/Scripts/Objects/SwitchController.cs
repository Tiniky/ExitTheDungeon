using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : Interactable {
    public Animator leverAnimator;
    public DoorController connectedDoor;
    public int pullCount;

    void Start() {
        pullCount = 0;
        isInRange = false;
        isStillInteractable = true;
        CutsceneManager.SetUpHeights(700, 900);
    }

    void Update() {
        UpdateTrigger();
    }

    protected override void Interact() {
        PullSwitch();
    }

    void PullSwitch() {
        pullCount++;

        if(pullCount == 1){
            connectedDoor.Open();
            GameManager.CutSceneTrigger();
            CutsceneManager.StartUp();
            StartCoroutine(SignalEndOfCutscene());
        }

        if(pullCount % 2 == 1){
            leverAnimator.SetBool("pulledToOpen", true);
            Debug.Log("open");
        } else{
            leverAnimator.SetBool("pulledToOpen", false);
            Debug.Log("close");
        }
    }

    private IEnumerator SignalEndOfCutscene() {
        yield return new WaitForSeconds(2.5f);
        CutsceneManager.WrapUp();
    }
}
