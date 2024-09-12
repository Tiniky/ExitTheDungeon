using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Button _button;
    private Action action;
    private GameObject _explanation;

    void Start() {
        _button = GetComponent<Button>();

        if (_button != null) {
            _button.onClick.AddListener(OnButtonClick);
        }
    }

    public Action InitializeAction(BasicAction actiontype){
        switch(actiontype){
            case BasicAction.ATTACK:
                action = new Attack();
                break;
            case BasicAction.DASH:
                action = new Dash();
                break;
            case BasicAction.RANGED:
                action = new Ranged();
                break;
            case BasicAction.SHOVE:
                action = new Shove();
                break;
        }

        return action;
    }

    private void OnButtonClick() {
        if(!action.IsWaitingForTarget()){
            ActiveAction();
        } else if(action.IsWaitingForTarget()) {
            DeactiveAction();
        }
    }

    public void ActiveAction(){
        if(BattleManager.CurrentFighter() is Adventurer){
            Debug.Log(action.ToString() + " activated");
            action.Activate();
        }
    }

    public void DeactiveAction(){
        if(BattleManager.CurrentFighter() is Adventurer){
            Debug.Log(action.ToString() + " deactivated");
            action.Deactivate();
        }
    }

    public Action GetAction(){
        return action;
    }

    public void ConnectExplanation(GameObject obj){
        _explanation = obj;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (Cursor.visible){
            _explanation.SetActive(true);
        }    
    }

    public void OnPointerExit(PointerEventData eventData) {
        _explanation.SetActive(false);
    }
}
