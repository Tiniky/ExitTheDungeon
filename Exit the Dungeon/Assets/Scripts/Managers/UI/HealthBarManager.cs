using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class HealthBarManager {
    private static GameObject _playerHBPrefab, _allyHBPrefabv1, _allyHBPrefabv2;
    private static Dictionary <Entity, GameObject> _entityHPBars;
    private static GameObject _HPHolder;

    public static void Initialize(){
        _playerHBPrefab = PrefabManager.PLAYER_HP;
        _allyHBPrefabv1 = PrefabManager.ALLY_HPv1;
        _allyHBPrefabv2 = PrefabManager.ALLY_HPv2;
        _entityHPBars = new Dictionary<Entity, GameObject>();

        _HPHolder = UIManager._CreateEmptyUIGameObject("HealthBarHolder", new Vector3(-600f, 425f, 0f));

        Adventurer playerEntity = GameManager.Player();
        InstantiateHealthBar(_playerHBPrefab, playerEntity);
    }

    private static void SetHealthBarIcon(GameObject healthbarObj, Entity entity){
        Image icon = healthbarObj.transform.Find("Icon").GetComponent<Image>();
        icon.sprite = entity.entityIcon.sprite;

        _entityHPBars[entity] = healthbarObj;
    }

    private static void InstantiateHealthBar(GameObject prefab, Entity entity, int i = 0){
        GameObject healthbar = PrefabManager.InstantiatePrefabV2(prefab, _HPHolder, false, new Vector3(0f, - i * 75f, 0f), entity.EntityName + "'s HealthBar");
        
        HealthBarController hpcontrol = healthbar.GetComponent<HealthBarController>();
        hpcontrol.SetMaxHP(entity.HP.GetMax());
        
        SetHealthBarIcon(healthbar, entity);
    }

    public static GameObject GetHealthBarOf(Entity entity){
        return _entityHPBars[entity];
    }

    public static void SetVisibility(bool shouldBeVisible) {
        _HPHolder.SetActive(shouldBeVisible);
    }

    public static GameObject CreateHealthBar(GameObject go){
        int index = GameManager.Allies().Count;
        Adventurer entity = go.GetComponent<Adventurer>();
        if(entity.Race == RaceType.DWARF || entity.Race == RaceType.ELF){
            InstantiateHealthBar(_allyHBPrefabv2, entity, index);
        } else{
            InstantiateHealthBar(_allyHBPrefabv1, entity, index);
        }
        

        return _entityHPBars[entity];
    }

    public static void UpdateHPFor(Entity entity){
        GameObject healthbar = _entityHPBars[entity];
        if(entity.isAlive){
            HealthBarController hpcontrol = healthbar.GetComponent<HealthBarController>();
            hpcontrol.SetHP(entity.HP.GetValue());
        } else{
            healthbar.SetActive(false);
        }
    }
}
