using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
    private bool _isActive;
    private Vector3 _motion;
    private float _duration, _wasShown;

    public void Show(){
        _isActive = true;
        _wasShown = Time.time;
    }

    public void DestroyText(){
        Destroy(gameObject);
    }

    public void UpdateTxt(){
        if(!_isActive){
            return;
        }

        if(Time.time - _wasShown > _duration){
            DestroyText();
        }

        gameObject.transform.position += _motion * Time.deltaTime;
    }

    public void InitMovement(Vector3 motion, float duration){
        _motion = motion;
        _duration = duration;
    }

    public Text GetText(){
        return gameObject.GetComponent<Text>();;
    }
}
