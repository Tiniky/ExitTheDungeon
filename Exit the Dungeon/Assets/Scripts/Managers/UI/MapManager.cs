using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapManager {
    private static GameObject _mapPrefab, _minimap;

    public static void Initialize(Canvas canvas){
        _mapPrefab = PrefabManager.MINIMAP;
        _minimap = PrefabManager.InstantiatePrefabV2(_mapPrefab, canvas.gameObject, false, new Vector3(-150f,-150f,0f), "Minimap");
    }

    public static void SetVisibility(bool shouldBeVisible) {
        _minimap.SetActive(shouldBeVisible);
    }
}