using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameMenuManager : MonoBehaviour {
    public Button _startButton, _backButton, _test;

    void Start(){
        _startButton.onClick.AddListener(StartGame);
        _backButton.onClick.AddListener(BackToMainMenu);
        _test.onClick.AddListener(Test);
    }

    private void StartGame(){
        ScenesManager.LoadGame();
    }

    private void BackToMainMenu(){
        ScenesManager.LoadMainMenu();
    }

    private void Test(){
        Debug.Log("Test");
    }
}
