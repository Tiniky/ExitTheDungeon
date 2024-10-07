using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public static class PrefabManager {
    // UI Prefabs
    public static GameObject PLAYER_HP;
    public static GameObject ALLY_HP;
    public static GameObject SKILL;
    public static GameObject BASIC_ACTION;
    public static GameObject ABILITY;
    public static GameObject MINIMAP;
    public static GameObject PASSIVE;
    public static GameObject GEM_COUNTER;
    public static GameObject FIGHT_PARTICIPANT_INITIATIVE;
    public static GameObject FIGHT_PARTICIPANT_QUEUE;
    public static GameObject TURN_PASS;
    public static GameObject ARROW;
    public static GameObject CUTSCENE;
    public static GameObject ABILITY_LEARNED_TEXT;
    public static GameObject INTERACTABLE_NEARBY_TEXT;
    public static GameObject THANKS_TEXT;
    public static GameObject FIGHT_TEXT;
    public static GameObject INITIATIVE;
    public static GameObject INVENTORY;
    public static GameObject INVENTORY_SLOT;
    public static GameObject LOG_UI;
    public static GameObject LOG_TEXT;
    public static GameObject EXPLANATION_BIG;
    public static GameObject EXPLANATION_SMALL;

    // UI Images
    public static Image IMG_ORC_TOKEN;
    public static Image IMG_HUMAN_TOKEN;
    public static Image IMG_ELF_TOKEN;
    public static Image IMG_DWARF_TOKEN;
    public static Image IMG_OGRE_TOKEN;
    public static Image IMG_ATTACK;
    public static Image IMG_BORING;
    public static Image IMG_CLUB;
    public static Image IMG_DASH;
    public static Image IMG_DOUBLE_STRIKE;
    public static Image IMG_MOVEMENT;
    public static Image IMG_RAGE;
    public static Image IMG_RANGED;
    public static Image IMG_RELENTLESS_ENDURANCE;
    public static Image IMG_SHOVE;
    public static Image IMG_SNEAK_ATTACK;
    public static Image IMG_DAGGER;
    public static Image IMG_BATTLE_AXE;
    public static Image IMG_NO_ARMOR;
    public static Image IMG_LIGHT_ARMOR;
    public static Image IMG_MEDIUM_ARMOR;
    public static Image IMG_HEAVY_ARMOR;
    public static Image IMG_SHORTBOW;

    // Creature Prefabs
    public static GameObject ORC_BARBARIAN;
    public static GameObject HUMAN_ROGUE;
    public static GameObject ELF_SORCERER;
    public static GameObject DWARF_CLERIC;
    public static List<GameObject> ALLIES;
    public static GameObject OGRE;
    public static GameObject GOBLIN;

    // Object Prefabs
    public static GameObject CHEST;
    public static GameObject DOOR;
    public static GameObject GEM;
    public static GameObject SCROLL;
    public static GameObject SWITCH;

    // Dungeon Prefabs
    public static GameObject TILE;

    // other
    public static Dictionary<string, string> EXPLANATIONS;
    public static List<string> GENERAL_ABILITIES;
    public static List<string> BARBARIAN_ABILITIES;
    public static List<string> CLERIC_ABILITIES;
    public static List<string> ROGUE_ABILITIES;
    public static List<string> SORCERER_ABILITIES;

    public static void Initialize(){
        PLAYER_HP = Resources.Load<GameObject>("Prefabs/UI/HealthBar_Player");
        ALLY_HP = Resources.Load<GameObject>("Prefabs/UI/HealthBar_Ally");
        SKILL = Resources.Load<GameObject>("Prefabs/UI/Skill");
        BASIC_ACTION = Resources.Load<GameObject>("Prefabs/UI/BasicAction");
        ABILITY = Resources.Load<GameObject>("Prefabs/UI/Ability");
        MINIMAP = Resources.Load<GameObject>("Prefabs/UI/Map");
        PASSIVE = Resources.Load<GameObject>("Prefabs/UI/Passive");
        GEM_COUNTER = Resources.Load<GameObject>("Prefabs/UI/GemCounter");
        FIGHT_PARTICIPANT_INITIATIVE = Resources.Load<GameObject>("Prefabs/UI/FightParticipant");
        FIGHT_PARTICIPANT_QUEUE = Resources.Load<GameObject>("Prefabs/UI/FightQueue");
        TURN_PASS = Resources.Load<GameObject>("Prefabs/UI/Turn");
        ARROW = Resources.Load<GameObject>("Prefabs/UI/Arrow");
        CUTSCENE = Resources.Load<GameObject>("Prefabs/UI/Cutscene");
        ABILITY_LEARNED_TEXT = Resources.Load<GameObject>("Prefabs/UI/AbilityLearnedText");
        INTERACTABLE_NEARBY_TEXT = Resources.Load<GameObject>("Prefabs/UI/InteractableNearbyText");
        THANKS_TEXT = Resources.Load<GameObject>("Prefabs/UI/ThanksText");
        FIGHT_TEXT = Resources.Load<GameObject>("Prefabs/UI/FightInfo");
        INITIATIVE = Resources.Load<GameObject>("Prefabs/UI/InitiativeRoll");
        INVENTORY = Resources.Load<GameObject>("Prefabs/UI/Inventory");
        INVENTORY_SLOT = Resources.Load<GameObject>("Prefabs/UI/CharSlot");
        LOG_UI = Resources.Load<GameObject>("Prefabs/UI/LogUI");
        LOG_TEXT = Resources.Load<GameObject>("Prefabs/UI/LogText");
        EXPLANATION_BIG = Resources.Load<GameObject>("Prefabs/UI/ExplanationBig");
        EXPLANATION_SMALL = Resources.Load<GameObject>("Prefabs/UI/ExplanationSmall");

        IMG_HUMAN_TOKEN = Resources.Load<Image>("Images/HumanToken");
        IMG_ORC_TOKEN = Resources.Load<Image>("Images/OrcToken");
        IMG_ELF_TOKEN = Resources.Load<Image>("Images/ElfToken");
        IMG_DWARF_TOKEN = Resources.Load<Image>("Images/DwarfToken");
        IMG_OGRE_TOKEN = Resources.Load<Image>("Images/OgreToken");
        IMG_ATTACK = Resources.Load<Image>("Prefabs/UI/Images/attack");
        IMG_BORING = Resources.Load<Image>("Prefabs/UI/Images/boring");
        IMG_CLUB = Resources.Load<Image>("Prefabs/UI/Images/club");
        IMG_DASH = Resources.Load<Image>("Prefabs/UI/Images/dash");
        IMG_DOUBLE_STRIKE = Resources.Load<Image>("Prefabs/UI/Images/double_strike");
        IMG_MOVEMENT = Resources.Load<Image>("Prefabs/UI/Images/movement");
        IMG_RAGE = Resources.Load<Image>("Prefabs/UI/Images/rage");
        IMG_RANGED = Resources.Load<Image>("Prefabs/UI/Images/ranged");
        IMG_RELENTLESS_ENDURANCE = Resources.Load<Image>("Prefabs/UI/Images/relentless_endurance");
        IMG_SHOVE = Resources.Load<Image>("Prefabs/UI/Images/shove");
        IMG_SNEAK_ATTACK = Resources.Load<Image>("Prefabs/UI/Images/sneak_attack");
        IMG_DAGGER = Resources.Load<Image>("Prefabs/UI/Images/dagger");
        IMG_BATTLE_AXE = Resources.Load<Image>("Prefabs/UI/Images/battle_axe");
        IMG_NO_ARMOR = Resources.Load<Image>("Prefabs/UI/Images/no_armor");
        IMG_MEDIUM_ARMOR = Resources.Load<Image>("Prefabs/UI/Images/medium_armor");
        IMG_LIGHT_ARMOR = Resources.Load<Image>("Prefabs/UI/Images/light_armor");
        IMG_HEAVY_ARMOR = Resources.Load<Image>("Prefabs/UI/Images/heavy_armor");
        IMG_SHORTBOW = Resources.Load<Image>("Prefabs/UI/Images/short_bow");
 
        ALLIES = new List<GameObject>();
        ORC_BARBARIAN = Resources.Load<GameObject>("Prefabs/Playable/Character_OrcBarbarian");
        ALLIES.Add(ORC_BARBARIAN);
        HUMAN_ROGUE = Resources.Load<GameObject>("Prefabs/Playable/Character_HumanRogue");
        ALLIES.Add(HUMAN_ROGUE);
        ELF_SORCERER = Resources.Load<GameObject>("Prefabs/Playable/Character_ElfSorcerer");
        ALLIES.Add(ELF_SORCERER);
        DWARF_CLERIC = Resources.Load<GameObject>("Prefabs/Playable/Character_DwarfCleric");
        ALLIES.Add(DWARF_CLERIC);

        OGRE = Resources.Load<GameObject>("Prefabs/Creatures/monster_ogre");
        GOBLIN = Resources.Load<GameObject>("Prefabs/Creatures/monster_goblin");

        CHEST = Resources.Load<GameObject>("Prefabs/Dungeon/Objects/chest");
        DOOR = Resources.Load<GameObject>("Prefabs/Dungeon/Objects/door");
        GEM = Resources.Load<GameObject>("Prefabs/Dungeon/Objects/gem");
        SCROLL = Resources.Load<GameObject>("Prefabs/Dungeon/Objects/scroll");
        SWITCH = Resources.Load<GameObject>("Prefabs/Dungeon/Objects/switch");

        TILE = Resources.Load<GameObject>("Prefabs/GameResources/Tile");
        Debug.Log("PrefabManager - initialized");

        EXPLANATIONS = new Dictionary<string, string>();
        FillExplanations();

        GENERAL_ABILITIES = new List<string>();
        BARBARIAN_ABILITIES = new List<string>();
        CLERIC_ABILITIES = new List<string>();
        ROGUE_ABILITIES = new List<string>();
        SORCERER_ABILITIES = new List<string>();
        FillAbilities();
    }

    public static GameObject InstantiatePrefabV1(GameObject prefab, GameObject parent, bool shouldAdd, Vector3 pos, string name){
        GameObject clone = GameObject.Instantiate(prefab, parent.transform);

        if(shouldAdd){
            clone.transform.position += pos;
        } else {
            clone.transform.position = pos;
        }

        clone.name = name;

        return clone;
    }

    public static GameObject InstantiatePrefabV2(GameObject prefab, GameObject parent, bool shouldAdd, Vector3 pos, string name){
        GameObject clone = GameObject.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);

        if(shouldAdd){
            clone.transform.position += pos;
        } else {
            clone.transform.position = pos;
        }

        clone.transform.SetParent(parent.transform, false);
        clone.name = name;

        return clone;
    }

    public static GameObject InstantiatePrefabV3(GameObject prefab, Transform parent){
        GameObject clone = GameObject.Instantiate(prefab, parent);
        return clone;
    }

    public static GameObject GetCharacterPrefab(string selected, bool isPlayer = false){
        GameObject prefab = null;
        switch(selected){
            case "OrcBarbarian":
                prefab = ORC_BARBARIAN;
                break;
            case "HumanRogue":
                prefab = HUMAN_ROGUE;
                break;
            case "ElfSorcerer":
                prefab = ELF_SORCERER;
                break;
            case "DwarfCleric":
                prefab = DWARF_CLERIC;
                break;
        }

        Debug.Log("PrefabManager - selected: " + selected);

        if(isPlayer){
            RemovePlayerFromAllies(prefab);
        }

        Debug.Log("PrefabManager - alliesNumber: " + ALLIES.Count);
        
        return prefab;
    }

    public static void RemovePlayerFromAllies(GameObject playerObj){
        ALLIES.Remove(playerObj);
    }

    public static Image GetImageOfAction(string actionName){
        switch(actionName){
            case "ATTACK":
                return IMG_ATTACK;
            case "DASH":
                return IMG_DASH;
            case "RANGED":
                return IMG_RANGED;
            case "SHOVE":
                return IMG_SHOVE;
            default:
                return null;
        }
    }

    public static Image GetImageOfAbility(string abilityName){
        switch(abilityName){
            case "Double Strike":
                return IMG_DOUBLE_STRIKE;
            case "Relentless Endurance":
                return IMG_RELENTLESS_ENDURANCE;
            case "Rage":
                return IMG_RAGE;
            case "Boring":
                return IMG_BORING;
            case "Sneak Attack":
                return IMG_SNEAK_ATTACK;
            default:
                return null;
        }
    }

    private static void FillExplanations(){
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("JSONs/explanatorytext");
        Debug.Log("PrefabManager - filling descriptions");

        if(jsonTextAsset != null){
            string content = jsonTextAsset.text;
            EXPLANATIONS = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
        }
    }

    private static void FillAbilities(){
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("JSONs/abilities");
        Debug.Log("PrefabManager - filling abilities");

        if(jsonTextAsset != null){
            string content = jsonTextAsset.text;
            var abilities = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);

            GENERAL_ABILITIES = abilities["general"];
            BARBARIAN_ABILITIES = abilities["barbarian"];
            CLERIC_ABILITIES = abilities["cleric"];
            ROGUE_ABILITIES = abilities["rogue"];
            SORCERER_ABILITIES = abilities["sorcerer"];

            Debug.Log("PrefabManager - general: " + string.Join(", ", GENERAL_ABILITIES));
            Debug.Log("PrefabManager - barbarian: " + string.Join(", ", BARBARIAN_ABILITIES));
            Debug.Log("PrefabManager - cleric: " + string.Join(", ", CLERIC_ABILITIES));
            Debug.Log("PrefabManager - rogue: " + string.Join(", ", ROGUE_ABILITIES));
            Debug.Log("PrefabManager - sorcerer: " + string.Join(", ", SORCERER_ABILITIES));
        }
    }
}
