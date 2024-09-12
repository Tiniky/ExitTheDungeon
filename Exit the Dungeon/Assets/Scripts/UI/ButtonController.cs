using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button button;
    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            OnButtonClick();
        }
    }

    private void OnButtonClick(){
        GameManager.StartCombatPhase();
        BattleManager.PressButton();
    }
}
