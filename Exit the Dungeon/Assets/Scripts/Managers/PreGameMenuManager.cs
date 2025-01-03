using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameMenuManager : MonoBehaviour {
    public Button _startButton, _backButton, _char1Button, _char2Button, _char3Button, _char4Button, _char5Button;
    public List<GameObject> stuff = new List<GameObject>();
    public List<Button> buttons = new List<Button>();
    private string _selectedCharacter;
    private Button _lastSelectedCharButton;
    private GameObject descPrefab;
    private Dictionary <GameObject, ExplanationController> _unlockDescriptions = new Dictionary<GameObject, ExplanationController>();

    void Start(){
        SaveManager.LoadGame();
        descPrefab = Resources.Load<GameObject>("Prefabs/UI/ExplanationSmall");
        SetUpUnlockConditions();

        _startButton.onClick.AddListener(StartGame);
        _backButton.onClick.AddListener(BackToMainMenu);
        _char1Button.onClick.AddListener(() => HandleCharSelectOrDeselect("OrcBarbarian", _char1Button));
        _char2Button.onClick.AddListener(() => HandleCharSelectOrDeselect("HumanRogue", _char2Button));
        _char3Button.onClick.AddListener(() => HandleCharSelectOrDeselect("ElfSorcerer", _char3Button));
        _char4Button.onClick.AddListener(() => HandleCharSelectOrDeselect("DwarfCleric", _char4Button));
        _char5Button.onClick.AddListener(() => HandleCharSelectOrDeselect("???", _char5Button));
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
            if(SaveManager.CheckIfUnlocked(AssetType.Character, character)){
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
    }

    private void SetUpUnlockConditions(){
        foreach(GameObject obj in stuff){
            Debug.Log("Setting up unlock condition for: " + obj.name);
            CreateDescription(obj);
        }

        foreach(Button button in buttons){
            Debug.Log("Setting up unlock condition for: " + button.gameObject.name);
            CreateDescription(button.gameObject);
        }
    }

    private void CreateDescription(GameObject obj){
        if(descPrefab == null){
            Debug.LogError("Description prefab is null");
            return;
        }

        MenuAssetHover asset = obj.AddComponent<MenuAssetHover>();        
        GameObject explanationText = PrefabManager.InstantiatePrefabV2(descPrefab, obj, false, new Vector3(0, 0, 0f), "ExplanationOf" + obj.name);
        ExplanationController ec = explanationText.GetComponent<ExplanationController>();
        asset.ConnectExplanation(explanationText, obj.name);
        ec.SetDescription(SaveManager.GetConditionOf(obj.name));
        explanationText.SetActive(false);
        _unlockDescriptions.Add(obj, ec);
    }
}