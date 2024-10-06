using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenesManager {

    public static void LoadScene(Scene scene){
        SceneManager.LoadScene(scene.ToString());
    }

    public static void LoadGame(){
        LoadScene(Scene.GameScene);
    }

    public static void LoadMainMenu(){
        LoadScene(Scene.MainMenu);
    }

    public static void LoadPreGame(bool isNew = false){
        if(isNew){
            //clear save file
        }

        LoadScene(Scene.PreGameScene);
    }
}

public enum Scene{
    MainMenu,
    GameScene,
    PreGameScene
}