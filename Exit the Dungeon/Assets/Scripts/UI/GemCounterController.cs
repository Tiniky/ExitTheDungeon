using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemCounterController : MonoBehaviour {
    private Text _gemText;
    private bool _isVisible;

    void Awake(){
        _gemText = gameObject.GetComponent<Text>();
        Debug.Log("gemtext " + _gemText);
        _isVisible = false;
    }

    public void AddGem(){
        if(!_isVisible){
            GameManager.Gem += 1;
        }

        _gemText.text = GameManager.Gem.ToString();
    }

    public void RemoveGems(){
        _gemText.text = "0";
    }
}
