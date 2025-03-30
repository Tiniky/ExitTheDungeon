using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour {
    public Button continueButton;

    void Start(){
        continueButton.onClick.AddListener(BackToMainMenu);
    }

    private void BackToMainMenu(){
        ScenesManager.LoadMainMenu();
    }
}
