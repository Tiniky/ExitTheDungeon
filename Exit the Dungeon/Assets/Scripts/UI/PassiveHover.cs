using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassiveHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private GameObject _explanation;

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
