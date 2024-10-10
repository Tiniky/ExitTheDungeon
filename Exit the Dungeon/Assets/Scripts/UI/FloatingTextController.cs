using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextController : MonoBehaviour {
    private GameObject _floattxt;
    private FloatingText floatTXT;
    private bool _isVisible;

    void Update(){
        if(_isVisible && floatTXT != null){
            floatTXT.UpdateTxt();
        } else if(_isVisible && floatTXT == null){
            Deactivate();
        }
    }

    public void Activate(Vector3 targetPos){
        _floattxt = TextUIManager.CreateFloatingText(targetPos);
        floatTXT = _floattxt.GetComponent<FloatingText>();
        _isVisible = false;
    }

    public void ShowText(string msg){
        if (_floattxt == null || floatTXT.GetText() == null) {
            Debug.LogError("Floating text parent or text component is null.");
            return;
        }

        floatTXT.GetText().text = msg;
        floatTXT.InitMovement(Vector3.up * 75, 1f);
        floatTXT.Show();
        _isVisible = true;
        Debug.Log("text triggered");
    }

    private void Deactivate(){
        _isVisible = false;
    }
}
