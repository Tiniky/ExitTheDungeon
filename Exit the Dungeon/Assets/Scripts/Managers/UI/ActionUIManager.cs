using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager {
    private static GameObject _actionPrefab, _descriptionPrefab;
    private static Dictionary<KeyCode, GameObject> _basicActions;
    private static Dictionary<Action, GameObject> _actions;
    private static Dictionary<BasicAction, ExplanationController> _descriptions;
    private static Dictionary<int, Vector3> positions;
    private static GameObject _actionHolder;
    
    public static void Initialize(){
        _actionPrefab = PrefabManager.BASIC_ACTION;
        _descriptionPrefab = PrefabManager.EXPLANATION_BIG;
        _basicActions = new Dictionary<KeyCode, GameObject>();
        _actions = new Dictionary<Action, GameObject>();
        _descriptions = new Dictionary<BasicAction, ExplanationController>();

        _actionHolder = UIManager._CreateEmptyUIGameObject("ActionHolder", new Vector3(-250f, -350f, 0f));

        positions = new Dictionary<int, Vector3>{
            {0, new Vector3(0f, 1f, 0f)},
            {1, new Vector3(-1f, 0f, 0f)},
            {2, new Vector3(1f, 0f, 0f)},
            {3, new Vector3(0f, -1f, 0f)}
        };

        for(int i = 0; i < Enum.GetValues(typeof(BasicAction)).Length; i++) {
            BasicAction currentAction = (BasicAction)Enum.GetValues(typeof(BasicAction)).GetValue(i);
            GameObject action = PrefabManager.InstantiatePrefabV2(_actionPrefab, _actionHolder, false, positions[i] * 60f, currentAction.ToString());
            FillActionData(action, currentAction);
        }
    }

    private static void FillActionData(GameObject actionObj, BasicAction actionType){
        string name = actionType.ToString();
        ActionButton button = actionObj.GetComponent<ActionButton>();
        Action action = button.InitializeAction(actionType);

        GameObject explanationText = PrefabManager.InstantiatePrefabV2(_descriptionPrefab, _actionHolder, false, new Vector3(300f, 25f, 0f), "ExplanationOf" + name);
        ExplanationController ec = explanationText.GetComponent<ExplanationController>();
        button.ConnectExplanation(explanationText);
        ec.SetUpDescription(PrefabManager.EXPLANATIONS[name], actionType);
        _descriptions[actionType] = ec;
        explanationText.SetActive(false);

        Image actionImage = actionObj.transform.Find("Icon").GetComponent<Image>();
        actionImage.sprite = PrefabManager.GetImageOfAction(name).sprite;
        _basicActions[Settings.GetKeyCodeOfAction(name)] = actionObj;
        _actions[action] = actionObj;
        OOAVisibiltyOf(actionObj, false);
    }

    public static GameObject GetActionPressed(KeyCode k){
        return _basicActions[k];
    }

    public static void SetVisibility(bool shouldBeVisible) {
        _actionHolder.SetActive(shouldBeVisible);
    }

    public static void ActivateActionWithKey(KeyCode k){
        GameObject actionButton = _basicActions[k];
        ActionButton button = actionButton.GetComponent<ActionButton>();
        Action action = button.GetAction();

        if(!action.WasUsed){
            button.ActiveAction();
        } else if(action.WasUsed && action.IsWaitingForTarget()) {
            button.DeactiveAction();
        } else {
            Debug.Log("action already used");
        }
    }

    public static void UpdateActions(){
        foreach(var elem in _descriptions){
            elem.Value.UpdateText(elem.Key);
        }
    }

    public static void OnOOA(Action action){
        OOAVisibiltyOf(_actions[action], true);
    }

    public static void ResetOOA(Action action){
        OOAVisibiltyOf(_actions[action], false);
    }

    private static void OOAVisibiltyOf(GameObject action, bool shouldBeVisible){
        GameObject cooldownUI = action.transform.Find("OutOfAction").gameObject;
        cooldownUI.SetActive(shouldBeVisible);
    }

    public static void ShowOOAForAll(){
        foreach(var elem in _actions){
            OnOOA(elem.Key); 
        }
    }

    public static void ResetOOAForAll(){
        foreach(var elem in _actions){
            ResetOOA(elem.Key); 
        }
    }
}
