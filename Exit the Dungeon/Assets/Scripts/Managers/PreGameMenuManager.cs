using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameMenuManager : MonoBehaviour {
    public Button _startButton, _backButton, _char1Button, _char2Button, _char3Button, _char4Button, _char5Button, _map1Button, _map2Button, _map3Button, _map4Button;
    public GameObject _item1Obj, _item2Obj, _item3Obj, _item4Obj, _item5Obj;
    private string _selectedCharacter;
    private Button _lastSelectedCharButton, _lastSelectedMapButton;

    void Start(){
        SaveManager.LoadGame();

        _startButton.onClick.AddListener(StartGame);
        _backButton.onClick.AddListener(BackToMainMenu);
        _char1Button.onClick.AddListener(() => HandleCharSelectOrDeselect("OrcBarbarian", _char1Button));
        _char2Button.onClick.AddListener(() => HandleCharSelectOrDeselect("HumanRogue", _char2Button));
        _char3Button.onClick.AddListener(() => HandleCharSelectOrDeselect("ElfSorcerer", _char3Button));
        _char4Button.onClick.AddListener(() => HandleCharSelectOrDeselect("DwarfCleric", _char4Button));
        _char5Button.onClick.AddListener(() => HandleCharSelectOrDeselect("???", _char5Button));
        _map1Button.onClick.AddListener(() => HandleMapSelectOrDeselect("Map1", _map1Button));
        _map2Button.onClick.AddListener(() => HandleMapSelectOrDeselect("Map2", _map2Button));
        _map3Button.onClick.AddListener(() => HandleMapSelectOrDeselect("Map3", _map3Button));
        _map4Button.onClick.AddListener(() => HandleMapSelectOrDeselect("Map4", _map4Button));
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

    private void HandleCharSelectOrDeselect(string character, Button button){
        if(_selectedCharacter == character && _lastSelectedCharButton == button){
            _selectedCharacter = "";
            _lastSelectedCharButton = null;
            ColorUtility.TryParseHtmlString("#FFFFFF", out Color defaultColor);
            button.image.color = defaultColor;
            Debug.Log("Deselected character: " + character);
        }else{
            if (_lastSelectedCharButton != null) {
                ColorUtility.TryParseHtmlString("#FFFFFF", out Color defaultColor);
                _lastSelectedCharButton.image.color = defaultColor;
            }
            _selectedCharacter = character;
            _lastSelectedCharButton = button;
            ColorUtility.TryParseHtmlString("#DCF096", out Color selectedColor);
            button.image.color = selectedColor;
            Debug.Log("Selected character: " + _selectedCharacter);
        }
    }

    private void HandleMapSelectOrDeselect(string map, Button button){
        if(_lastSelectedMapButton == button){
            _lastSelectedMapButton = null;
            ColorUtility.TryParseHtmlString("#FFFFFF", out Color defaultColor);
            button.image.color = defaultColor;
            Debug.Log("Deselected map: " + map);
        }else{
            if (_lastSelectedMapButton != null) {
                ColorUtility.TryParseHtmlString("#FFFFFF", out Color defaultColor);
                _lastSelectedMapButton.image.color = defaultColor;
            }
            _lastSelectedMapButton = button;
            ColorUtility.TryParseHtmlString("#DCF096", out Color selectedColor);
            button.image.color = selectedColor;
            Debug.Log("Selected map: " + map);
        }
    }
}