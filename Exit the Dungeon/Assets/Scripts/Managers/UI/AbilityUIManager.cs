using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AbilityUIManager {
    private static GameObject _abilityPrefab, _descriptionPrefab;
    private static Dictionary<KeyCode, GameObject> _abilityKeyPairs;
    private static Dictionary<Ability, GameObject> _abilities;
    private static List<ExplanationController> _descriptions;
    private static GameObject _abilityHolder;
    private static AbilityButtonHandler _handler;
    private static Adventurer _current;

    public static void Initialize(){
        _abilityPrefab = PrefabManager.ABILITY;
        _descriptionPrefab = PrefabManager.EXPLANATION_BIG;
        _abilityKeyPairs = new Dictionary<KeyCode, GameObject>();
        _abilities = new Dictionary<Ability, GameObject>();
        _descriptions = new List<ExplanationController>();

        _abilityHolder = UIManager._CreateEmptyUIGameObject("AbilityHolder", new Vector3(-75f, -425f, 0f));
        _handler = _abilityHolder.AddComponent<AbilityButtonHandler>();

        for(int i = 0; i < 4; i++){
            GameObject ability = PrefabManager.InstantiatePrefabV2(_abilityPrefab, _abilityHolder, false, new Vector3(i * 85f, 0f, 0f), "Ability");
            _abilityKeyPairs[Settings.GetKeyCodeOfAbility(i)] = ability;
        }

        FillAbilitiesOf(GameManager.Player());
    }

    public static void FillAbilitiesOf(Adventurer adventurer){
        if(adventurer != _current){
            foreach(var elem in _abilities){
                elem.Value.name = "Ability";
            }
            
            _abilities.Clear();
            _descriptions.Clear();

            List<Ability> activesOfCurrent = adventurer.GetActives();
            int abilities = 0;

            foreach(var elem in _abilityKeyPairs){
                GameObject abilityObj = elem.Value;

                if(abilities < activesOfCurrent.Count && activesOfCurrent[abilities] != null){
                    FillAbility(abilityObj, activesOfCurrent[abilities]);
                } else{
                    abilityObj.GetComponent<Button>().enabled = false;
                    abilityObj.transform.Find("Border/Icon").gameObject.SetActive(false);
                }

                abilities += 1;
                _handler.SetUpActiveAbilityVisbility();

                OOAVisibiltyOf(abilityObj, false);
                CDVisibiltyOf(abilityObj, false);
                CAVisibiltyOf(abilityObj, false);
            }
            
            _current = adventurer;
        }
    }

    public static void RefreshAbilities(Adventurer adventurer){
        _current = null;
        FillAbilitiesOf(adventurer);
    }

    private static void FillAbility(GameObject abilityObj, Ability ability){
        string name = ability.DisplayName;
        GameObject imageHolder = abilityObj.transform.Find("Border/Icon").gameObject;
        imageHolder.SetActive(true);
        abilityObj.name = name;
        abilityObj.GetComponent<Button>().enabled = true;
        Image abilityImage = imageHolder.GetComponent<Image>();
        abilityImage.sprite = PrefabManager.GetImageOfAbility(name).sprite;

        AbilityButton button = abilityObj.GetComponent<AbilityButton>();
        button.InitializeButton(ability);

        GameObject explanationText = PrefabManager.InstantiatePrefabV2(_descriptionPrefab, _abilityHolder, false, new Vector3(125f, 100f, 0f), "ExplanationOf" + name);
        ExplanationController ec = explanationText.GetComponent<ExplanationController>();
        button.ConnectExplanation(explanationText);
        ec.SetDescription(PrefabManager.EXPLANATIONS[ability.DisplayName]);
        _descriptions.Add(ec);
        explanationText.SetActive(false);

        _abilities[ability] = abilityObj;
    }

    public static GameObject GetAbilityPressed(KeyCode k){
        return _abilityKeyPairs[k];
    }

    public static void SetVisibility(bool shouldBeVisible) {
        _abilityHolder.SetActive(shouldBeVisible);
    }

    public static void ActivateAbilityWithKey(KeyCode k){
        GameObject abilityButton = _abilityKeyPairs[k];
        AbilityButton button = abilityButton.GetComponent<AbilityButton>();
        button.ActiveAbility();
    }

    public static void OnCD(Ability ability){
        CDVisibiltyOf(_abilities[ability], true);
    }

    public static void ResetCD(Ability ability){
        CDVisibiltyOf(_abilities[ability], false);
    }

    private static void CDVisibiltyOf(GameObject ability, bool shouldBeVisible){
        GameObject cooldownUI = ability.transform.Find("State/OnCooldown").gameObject;
        cooldownUI.SetActive(shouldBeVisible);
    }

    public static void OnCA(Ability ability){
        CAVisibiltyOf(_abilities[ability], true);
    }

    public static void ResetCA(Ability ability){
        CAVisibiltyOf(_abilities[ability], false);
    }

    private static void CAVisibiltyOf(GameObject ability, bool shouldBeVisible){
        GameObject cooldownUI = ability.transform.Find("State/CurrentlyActive").gameObject;
        cooldownUI.SetActive(shouldBeVisible);
    }

    public static void OnOOA(Ability ability){
        OOAVisibiltyOf(_abilities[ability], true);
    }

    public static void ResetOOA(Ability ability){
        OOAVisibiltyOf(_abilities[ability], false);
    }

    private static void OOAVisibiltyOf(GameObject ability, bool shouldBeVisible){
        GameObject cooldownUI = ability.transform.Find("State/OutOfAction").gameObject;
        cooldownUI.SetActive(shouldBeVisible);
    }

    public static void ShowOOAForAll(){
        foreach(var elem in _abilities){
            OnOOA(elem.Key); 
        }
    }

    public static void ShowOOAForNonCombat(){
        foreach(var elem in _abilities){
            if(!(elem.Key.Type == AbilityType.ACTIVE && elem.Key.Effect == AbilityEffect.DAMAGE)){
                OnOOA(elem.Key); 
            }
        }
    }

    public static void ResetOOAForAll(){
        foreach(var elem in _abilities){
            ResetOOA(elem.Key); 
        }
    }
}