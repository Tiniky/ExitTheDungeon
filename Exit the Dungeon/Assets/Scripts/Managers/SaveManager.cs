using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class SaveManager {
    
    public static TextAsset jsonFile;

    public static void LoadGame(){
        jsonFile = Resources.Load<TextAsset>("JSONs/saveFile");

        if(jsonFile == null){
            Debug.LogError("JSON file not found.");
            return;
        }

        JObject loadData = JObject.Parse(jsonFile.text);
    }
}
