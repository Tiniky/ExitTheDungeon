using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class EscapeMenuController : MonoBehaviour {
    public Button _resume, _quit, _wipeSave, _terminateRun;
    private static TextAsset jsonSaveFile;
    private static TextAsset jsonSaveFileOG;

    void Start(){
        _resume.onClick.AddListener(ResumeGame);
        _quit.onClick.AddListener(QuitGame);
        _wipeSave.onClick.AddListener(WipeSave);
        _terminateRun.onClick.AddListener(TerminateRun);
    }

    private void ResumeGame(){
        GameManager.TogglePause();
    }

    private void QuitGame(){
        Application.Quit();
    }

    private void WipeSave(){
        jsonSaveFileOG = Resources.Load<TextAsset>("JSONs/saveOG");

        if(jsonSaveFileOG == null){
            Debug.LogError("Original JSON save file not found.");
            return;
        }

        string path = Path.Combine(Application.dataPath, "Resources/JSONs/saveFile.json");

        try {
            File.WriteAllText(path, jsonSaveFileOG.text);
            Debug.Log("Save file successfully wiped.");
        } catch (IOException e) {
            Debug.LogError("Failed to write to save file: " + e.Message);
        }
    }

    private void TerminateRun(){
        //GameManager.TerminateRun();
    }
}
