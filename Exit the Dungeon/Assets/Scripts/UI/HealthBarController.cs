using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {
    public Text hpText;
    public Slider hpSlider;
    private int _maxPossibleHP;

    public void SetMaxHP(int maxHP){
        _maxPossibleHP = maxHP;
        hpSlider.maxValue = _maxPossibleHP;
        hpSlider.value = _maxPossibleHP;
        hpText.text = string.Format("{0}/{1}", _maxPossibleHP, _maxPossibleHP);
    }

    public void SetHP(int HP){
        Debug.Log("value: " + HP + ", max: " + _maxPossibleHP);
        hpSlider.value = HP;
        hpText.text = string.Format("{0}/{1}", HP, _maxPossibleHP);
    }
}
