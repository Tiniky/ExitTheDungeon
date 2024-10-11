using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class InventoryManager {
    private static GameObject _inventoryPrefab, _inventorySlotPrefab, _inventory;
    private static List<GameObject> _inventoryParts;
    private static string[] _equipmentTypes;
    private static bool _isVisible;
    private static Dictionary<int, Vector3> positions;
    private static Color _active, _passive;
    private static GameObject _inventoryHolder;

    public static void Initialize(Canvas canvas){
        _inventoryPrefab = PrefabManager.INVENTORY;
        _inventorySlotPrefab = PrefabManager.INVENTORY_SLOT;
        _inventoryParts = new List<GameObject>();
        _equipmentTypes = new string[] {"Armor", "Melee", "Ranged"};
        _isVisible = true;

        _inventory = PrefabManager.InstantiatePrefabV2(_inventoryPrefab, canvas.gameObject, false, new Vector3(0f, 0f, 0f), "Inventory");
        InventoryController colors = _inventory.GetComponent<InventoryController>();
        _active = colors.active;
        _passive = colors.passive;

        positions = new Dictionary<int, Vector3>{
            {0, new Vector3(-1f, 1f, 0f)},
            {1, new Vector3(1f, 1f, 0f)},
            {2, new Vector3(-1f, -1f, 0f)},
            {3, new Vector3(1f, -1f, 0f)}
        };

        for(int i = 0; i < 4; i++){
            float distance = 270;
            GameObject inventorySlot = PrefabManager.InstantiatePrefabV2(_inventorySlotPrefab, _inventory, false, positions[i] * distance, "InventorySlot" + i);
            _inventoryParts.Add(inventorySlot);
            ChangeBackground(i, false);
        }

        
        FillSectionWithData(_inventoryParts[0], GameManager.Player());
    }

    private static void FillSectionWithData(GameObject section, Adventurer adventurer){
        Text name = section.transform.Find("CharName").GetComponent<Text>();
        name.text = adventurer.EntityName;
        Image character = section.transform.Find("Character").GetComponent<Image>();
        character.sprite = adventurer.gameObject.GetComponent<SpriteRenderer>().sprite;
        
        switch(adventurer.Race){
            case RaceType.HUMAN:
                character.transform.localScale = new Vector3(1.45f,2.1f,1f);
                break;
            case RaceType.ELF:
                character.transform.localScale = new Vector3(3f,3f,1f);
                break;
            case RaceType.DWARF:
                character.transform.localScale = new Vector3(3f,2.5f,1f);
                break;
            case RaceType.ORC:
                character.transform.localScale = new Vector3(3f,3f,1f);
                break;
        }

        Transform ac = section.transform.Find("ArmorClass/Value");
        FillAC(ac, adventurer);
        Transform equipments = section.transform.Find("Equipments");
        FillEquipments(equipments, adventurer);
        section.GetComponent<Image>().color = _active;
    }

    private static void FillAC(Transform armorClass, Adventurer adventurer){
        Text value = armorClass.GetComponent<Text>();
        value.text = adventurer.AC.GetValue().ToString();
    }

    private static void FillEquipments(Transform equipments, Adventurer adventurer){
        foreach(string equipmentType in _equipmentTypes){
            Transform equipment = equipments.Find(equipmentType);

            Image icon = equipment.Find("Border/Icon").GetComponent<Image>();
            Text label = equipment.Find("Label").GetComponent<Text>();
            Text value = equipment.Find("Value").GetComponent<Text>();
        
            switch(equipmentType){
                case "Armor":
                    FillArmor(icon, label, value, adventurer);
                    break;
                case "Melee":
                    FillMelee(icon, label, value, adventurer);
                    break;
                case "Ranged":
                    FillRanged(icon, label, value, adventurer);
                    break;
                default:
                    break;
            }
        }
    }

    private static void FillArmor(Image icon, Text label, Text value, Adventurer adventurer){
        switch(adventurer.Armor){
            case ArmorType.UNARMORED:
                icon.sprite = PrefabManager.IMG_NO_ARMOR.sprite;
                label.text = "NO ARMOR";
                value.text = "+0 to AC";
                break;
            case ArmorType.LIGHT:
                icon.sprite = PrefabManager.IMG_LIGHT_ARMOR.sprite;
                label.text = "LIGHT ARMOR";
                value.text = "+2 to AC";
                break;
            case ArmorType.MEDIUM:
                icon.sprite = PrefabManager.IMG_MEDIUM_ARMOR.sprite;
                label.text = "MEDIUM ARMOR";
                value.text = "+4 to AC";
                break;
            case ArmorType.HEAVY:
                icon.sprite = PrefabManager.IMG_HEAVY_ARMOR.sprite;
                label.text = "HEAVY ARMOR";
                value.text = "+8 to AC";
                break;
        }
    }

    private static void FillMelee(Image icon, Text label, Text value, Adventurer adventurer){
        Weapon melee = adventurer.Melee;
        icon.sprite = melee.Icon.sprite;
        label.text = melee.WeaponName;
        value.text = melee.DMGmult.ToString() + melee.DMG.ToString();
    }

    private static void FillRanged(Image icon, Text label, Text value, Adventurer adventurer){
        Weapon ranged = adventurer.Ranged;
        
        if(ranged != null){
            icon.sprite = ranged.Icon.sprite;
            label.text = ranged.WeaponName;
            value.text = ranged.DMGmult.ToString() + ranged.DMG.ToString();
        } else {
            icon.color = Color.gray;
            label.text = "NO RANGED";
            value.text = "-";
        }
    }

    public static void AddAllyToInventory(int index){
        ChangeBackground(index, true);
        List<GameObject> allies = GameManager.Allies();
        FillSectionWithData(_inventoryParts[index].gameObject, allies[index-1].GetComponent<Adventurer>());
    }

    public static void InventoryVisibility(){
        _isVisible = !_isVisible;
        _inventory.SetActive(_isVisible);
        UIManager.UIVisibility(!_isVisible);
        SkillUIManager.SetVisibility(_isVisible);
        GemUIManager.SetVisibility(_isVisible);
        
        if(GameManager.Phase == GamePhase.ADVENTURE){
            Cursor.visible = _isVisible;
        }
    }

    private static void ChangeBackground(int index, bool shouldBeActive){
        if(shouldBeActive){
            _inventoryParts[index].GetComponent<Image>().color = _active;
        } else {
            _inventoryParts[index].GetComponent<Image>().color = _passive;   
        }
    }
}
