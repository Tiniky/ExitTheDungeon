using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    public Button _continueButton, _newGameButton, _exitButton;

    void Start(){
        _continueButton.onClick.AddListener(LoadSavedGame);
        
        //need to check if there is a save file
        _continueButton.interactable = false;

        _newGameButton.onClick.AddListener(StartNewGame);
        _exitButton.onClick.AddListener(ExitGame);
    }

    private void LoadSavedGame(){
        ScenesManager.LoadPreGame();
    }

    private void StartNewGame(){
        ScenesManager.LoadPreGame(true);
    }

    private void ExitGame(){
        Application.Quit();
    }
}
