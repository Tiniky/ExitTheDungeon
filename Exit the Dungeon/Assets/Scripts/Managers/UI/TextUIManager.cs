using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextUIManager {
    private static GameObject _thanksTextPrefab, _interactableTextPrefab, _scrollTextPrefab;
    private static GameObject _textHolder, _interactableTXT;
    private static FloatingTextController _ftc;

    public static void Initialize(){
        _thanksTextPrefab = PrefabManager.THANKS_TEXT;
        _interactableTextPrefab = PrefabManager.INTERACTABLE_NEARBY_TEXT;
        _scrollTextPrefab = PrefabManager.ABILITY_LEARNED_TEXT;

        _textHolder = UIManager._CreateEmptyUIGameObject("TextHolder", new Vector3(0f, 0f, 0f));
        _ftc = _textHolder.AddComponent<FloatingTextController>();
        _interactableTXT = PrefabManager.InstantiatePrefabV2(_interactableTextPrefab, _textHolder, false, new Vector3(30f, 55f, 0f), "Interactable");
    
    }

    public static GameObject CreateFloatingText(Vector3 pos){
        return PrefabManager.InstantiatePrefabV2(_thanksTextPrefab, _textHolder, false, pos, "FloatingText");
    }

    public static void UpdateInteractableText(bool isInRange){
        _interactableTXT.SetActive(isInRange);
    }

    public static GameObject CreateScrollText(string abilityName){
        GameObject scrollText = PrefabManager.InstantiatePrefabV2(_scrollTextPrefab, _textHolder, false, new Vector3(50f, 75f, 0f), "Scroll");
        scrollText.GetComponent<Text>().text = abilityName;
        return scrollText;
    }

    public static FloatingTextController GetFTC(){
        return _ftc;
    }
}
