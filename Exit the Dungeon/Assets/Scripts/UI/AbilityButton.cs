using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Button _button;
    private Image _border;
    private Ability _ability;
    private GameObject _explanation;

    void Start() {
        _button = GetComponent<Button>();

        if (_button != null) {
            _button.onClick.AddListener(OnButtonClick);
        }
    }

    public void InitializeButton(Ability ability){
        _ability = ability;
        Debug.Log(_ability.DisplayName);
    }

    public void SetVisibility(Color color) {
        _border = transform.Find("Border").GetComponent<Image>();

        if(_border != null){
            _border.color = color;
        }
    }

    private void OnButtonClick() {
        ActiveAbility();
    }

    public void ActiveAbility(){
        if(BattleManager.CurrentFighter() is Adventurer){
            if(_ability.State == AbilityState.READY){
                Debug.Log(_ability.DisplayName + " activated");
                _ability.Activate();
            } else if(_ability.State == AbilityState.ACTIVE){
                Debug.Log(_ability.DisplayName + " deactivated");
                _ability.Deactivate();
            }
        }
    }

    public void ConnectExplanation(GameObject obj){
        _explanation = obj;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(_explanation != null && Cursor.visible){
            _explanation.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
       if(_explanation != null){
            _explanation.SetActive(false);
        }
    }
}
