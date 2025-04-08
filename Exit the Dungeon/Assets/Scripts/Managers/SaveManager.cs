using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class SaveManager {
    
    private static TextAsset jsonFile, saveFile;
    public static List<SaveAsset> Characters = new List<SaveAsset>();
    public static List<SaveAsset> Items = new List<SaveAsset>();
    public static List<SaveAsset> Maps = new List<SaveAsset>();
    public static Dictionary<string, string> PlayerData = new Dictionary<string, string>();
    public static bool WasLoaded = false;

    public static void LoadGame(){
        jsonFile = Resources.Load<TextAsset>("JSONs/unlockFile");
        saveFile = Resources.Load<TextAsset>("JSONs/saveFile");

        if(jsonFile == null || saveFile == null){
            Debug.LogError("JSON file not found.");
            return;
        }

        JObject loadData = JObject.Parse(jsonFile.text);
        JObject saveData = JObject.Parse(saveFile.text);

        string assetName, assetCondition;
        bool assetUnlocked;
        
        foreach(var character in loadData["characters"]){
            assetName = character["name"].ToString();
            assetUnlocked = (bool)character["unlocked"];
            assetCondition = character["condition"].ToString();
            Characters.Add(new SaveAsset(assetName, assetUnlocked, assetCondition));
        }

        foreach(var item in loadData["items"]){
            assetName = item["name"].ToString();
            assetUnlocked = (bool)item["unlocked"];
            assetCondition = item["condition"].ToString();
            Items.Add(new SaveAsset(assetName, assetUnlocked, assetCondition));
        }

        foreach(var map in loadData["maps"]){
            assetName = map["name"].ToString();
            assetUnlocked = (bool)map["unlocked"];
            assetCondition = map["condition"].ToString();
            Maps.Add(new SaveAsset(assetName, assetUnlocked, assetCondition));
        }

        foreach(var stat in saveData){
            PlayerData[stat.Key] = stat.Value.ToString();
        }
        
        WasLoaded = true;
        DebugAll();
    }

    private static void DebugAll(){
        foreach(var character in Characters){
            Debug.Log("Character: " + character.Name + " - Unlocked: " + character.Unlocked + " - Condition: " + character.Condition);
        }

        foreach(var item in Items){
            Debug.Log("Item: " + item.Name + " - Unlocked: " + item.Unlocked + " - Condition: " + item.Condition);
        }

        foreach(var map in Maps){
            Debug.Log("Map: " + map.Name + " - Unlocked: " + map.Unlocked + " - Condition: " + map.Condition);
        }

        foreach(var stat in PlayerData){
            Debug.Log("Stat: " + stat.Key + " - Value: " + stat.Value);
        }
    }

    public static void HandleIfUnlocked(AssetType at, GameObject gameObject){
        string objectName = gameObject.name;
        
        if(CheckIfUnlocked(at, objectName)){
            Transform chains = gameObject.transform.Find("CHAINS");
            if(chains != null){
                GameObject childGameObject = chains.gameObject;
                childGameObject.SetActive(false);
            }
        }
    }

    public static bool CheckIfUnlocked(AssetType assetType, string objectName){
        switch(assetType){
            case AssetType.Character:
                SaveAsset character = Characters.Find(c => c.Name == objectName);
                if(character != null){
                    return character.Unlocked;
                }
                break;
            case AssetType.Item:
                SaveAsset item = Items.Find(i => i.Name == objectName);
                if(item != null){
                    return item.Unlocked;
                }
                break;
            case AssetType.Map:
                SaveAsset map = Maps.Find(m => m.Name == objectName);
                if(map != null){
                    return map.Unlocked;
                }
                break;
        }
        return false;
    }

    public static string GetConditionOf(string name){
        SaveAsset asset = GetAsset(name);

        if(asset != null){
            return asset.Condition;
        }

        return "";
    }

    public static SaveAsset GetAsset(string name){
        SaveAsset asset = Characters.Find(c => c.Name == name);
        if(asset == null){
            asset = Items.Find(i => i.Name == name);
            if(asset == null){
                asset = Maps.Find(m => m.Name == name);
            }
        }

        return asset;
    }

    public static void SaveProgress(){
        Dictionary<string, string> newPlayerData = GameManager.GetPlayerData();
        JObject loadData = JObject.Parse(saveFile.text);

        LogManager.AddMessage("save in progress");

        foreach (var stat in newPlayerData) {
            LogManager.AddMessage("Stat: " + stat.Key + " - Value: " + stat.Value);
            loadData[stat.Key] = JToken.FromObject(stat.Value);
        }

        LogManager.AddMessage("out of the loop");

        string updatedJson = loadData.ToString();
        LogManager.AddMessage("save in progress 2");
        File.WriteAllText("Assets/Resources/JSONs/saveFile.json", updatedJson);
        LogManager.AddMessage("JSON updated successfully.");
        Debug.Log("Progress saved successfully.");
    }
}
