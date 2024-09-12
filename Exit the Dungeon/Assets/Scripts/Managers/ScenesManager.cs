using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenesManager {

    public static void LoadScene(Scene scene){
        SceneManager.LoadScene(scene.ToString());
    }

    public static void LoadNewDemoGame(){
        LoadScene(Scene.DemoScene);
    }

    public static void LoadMainMenu(){
        LoadScene(Scene.MainMenu);
    }
}

public enum Scene{
    MainMenu,
    DemoScene
}