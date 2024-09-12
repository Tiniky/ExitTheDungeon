using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnPassButton : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData){
        if (eventData.button == PointerEventData.InputButton.Left){
            BattleManager.GoNext();
            Debug.Log("Turn passed");
        }
    }
}