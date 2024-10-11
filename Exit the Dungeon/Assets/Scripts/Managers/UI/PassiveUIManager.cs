using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PassiveUIManager {
    private static GameObject _passivePrefab, _descriptionPrefab;
    private static Dictionary<Ability, GameObject> _abilities;
    private static List<ExplanationController> _descriptions;
    private static GameObject _passiveHolder;
    private static Adventurer _current;

    public static void Initialize(){
        _passivePrefab = PrefabManager.PASSIVE;
        _descriptionPrefab = PrefabManager.EXPLANATION_BIG;
        _abilities = new Dictionary<Ability, GameObject>();
        _descriptions = new List<ExplanationController>();
    
        _passiveHolder = UIManager._CreateEmptyUIGameObject("PassiveHolder", new Vector3(600f, 450f, 0f));

        FillPassiveOf(GameManager.Player());
    }

    public static void FillPassiveOf(Adventurer adventurer){
        if(adventurer != _current){
            foreach(var elem in _abilities){
                GameManager.DestroyObj(elem.Value);
            }

            _abilities.Clear();
            List<Ability> passivesOfCurrent = adventurer.GetPassives();

            foreach(Ability passive in passivesOfCurrent){
                CreateUIForPassive(passive);
            }

            _current = adventurer;
        }
    }

    public static void CreateUIForPassive(Ability ability){
        Debug.Log("PassiveUIManager - " + ability.DisplayName);
        int distance = _abilities.Count;

        GameObject passive = PrefabManager.InstantiatePrefabV2(_passivePrefab, _passiveHolder, false, new Vector3(distance * (-85f), 0f, 0f), ability.DisplayName);
        FillPassiveData(passive, ability);
    }

    private static void FillPassiveData(GameObject passiveObj, Ability ability){
        Image passiveImage = passiveObj.transform.Find("Icon").GetComponent<Image>();
        passiveImage.sprite = ability.Icon.sprite;

        PassiveHover ph = passiveObj.GetComponent<PassiveHover>();
        GameObject explanationText = PrefabManager.InstantiatePrefabV2(_descriptionPrefab, _passiveHolder, false, new Vector3(-125f, -100f, 0f), "ExplanationOf" + ability.DisplayName);
        ExplanationController ec = explanationText.GetComponent<ExplanationController>();
        ph.ConnectExplanation(explanationText);
        ec.SetDescription(PrefabManager.EXPLANATIONS[ability.DisplayName]);
        _descriptions.Add(ec);
        explanationText.SetActive(false);

        _abilities[ability] = passiveObj;
        CDVisibiltyOf(passiveObj, false);
    }

    public static void SetVisibility(bool shouldBeVisible) {
        _passiveHolder.SetActive(shouldBeVisible);
    }

    public static void OnCD(Ability ability){
        CDVisibiltyOf(_abilities[ability], true);
    }

    public static void ResetCD(Ability ability){
        CDVisibiltyOf(_abilities[ability], false);
    }

    private static void CDVisibiltyOf(GameObject passive, bool shouldBeVisible){
        GameObject cooldownUI = passive.transform.Find("OnCooldown").gameObject;
        cooldownUI.SetActive(shouldBeVisible);
    }
}
