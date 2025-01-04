using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuController : MonoBehaviour {
    public Button _resume, _quit;

    void Start(){
        _resume.onClick.AddListener(ResumeGame);
        _quit.onClick.AddListener(QuitGame);
    }

    private void ResumeGame(){
        GameManager.TogglePause();
    }

    private void QuitGame(){
        Application.Quit();
    }
}
