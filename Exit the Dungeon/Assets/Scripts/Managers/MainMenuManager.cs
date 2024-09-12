using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    public Button _newGameButton, _exitButton;

    void Start(){
        _newGameButton.onClick.AddListener(StartNewGame);
        _exitButton.onClick.AddListener(ExitGame);
    }

    private void StartNewGame(){
        ScenesManager.LoadNewDemoGame();
    }

    private void ExitGame(){
        Application.Quit();
    }
}
