using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenesManager {

    public static void LoadScene(Scene scene){
        SceneManager.LoadScene(scene.ToString());
    }

    public static void LoadGame(){
        LoadScene(Scene.DemoScene);
    }

    public static void LoadMainMenu(){
        LoadScene(Scene.MainMenu);
    }

    public static void LoadPreGame(){
        LoadScene(Scene.PreGameScene);
    }

    public static void LoadDeathScreen(){
        LoadScene(Scene.DeathScreenScene);
    }
}

public enum Scene{
    MainMenu,
    DemoScene,
    PreGameScene,
    DeathScreenScene
}