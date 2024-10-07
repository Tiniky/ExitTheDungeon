using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameMenuManager : MonoBehaviour {
    public Button _startButton, _backButton, _char1Button, _char2Button, _char3Button, _char4Button;
    private string _selectedCharacter;
    private Button _lastSelectedButton;

    void Start(){
        _startButton.onClick.AddListener(StartGame);
        _backButton.onClick.AddListener(BackToMainMenu);
        _char1Button.onClick.AddListener(() => HandleSelectOrDeselect("OrcBarbarian", _char1Button));
        _char2Button.onClick.AddListener(() => HandleSelectOrDeselect("HumanRogue", _char2Button));
        _char3Button.onClick.AddListener(() => HandleSelectOrDeselect("ElfSorcerer", _char3Button));
        _char4Button.onClick.AddListener(() => HandleSelectOrDeselect("DwarfCleric", _char4Button));
    }

    private void StartGame(){
        if(!string.IsNullOrEmpty(_selectedCharacter)){
            GameManager.SelectedCharacter = _selectedCharacter;
            ScenesManager.LoadGame();
        }else{
            Debug.Log("Please select a character");
        }
    }

    private void BackToMainMenu(){
        ScenesManager.LoadMainMenu();
    }

    private void HandleSelectOrDeselect(string character, Button button){
        if(_selectedCharacter == character && _lastSelectedButton == button){
            _selectedCharacter = "";
            _lastSelectedButton = null;
            ColorUtility.TryParseHtmlString("#FFFFFF", out Color defaultColor);
            button.image.color = defaultColor;
            Debug.Log("Deselected character: " + character);
        }else{
            if (_lastSelectedButton != null) {
                ColorUtility.TryParseHtmlString("#FFFFFF", out Color defaultColor);
                _lastSelectedButton.image.color = defaultColor;
            }
            _selectedCharacter = character;
            _lastSelectedButton = button;
            ColorUtility.TryParseHtmlString("#DCF096", out Color selectedColor);
            button.image.color = selectedColor;
            Debug.Log("Selected character: " + _selectedCharacter);
        }
    }
}