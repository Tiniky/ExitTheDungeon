using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GemUIManager {
    private static GameObject _gemPrefab, _gemCounter;

    public static void Initialize(Canvas canvas){
        _gemPrefab = PrefabManager.GEM_COUNTER;
        _gemCounter = PrefabManager.InstantiatePrefabV2(_gemPrefab, canvas.gameObject, false, new Vector3(800f,-425f,0f), "Gem Counter");
    }

    public static void SetVisibility(bool shouldBeVisible) {
        _gemCounter.SetActive(shouldBeVisible);
    }

    public static void Add(){
        GemCounterController gcc = _gemCounter.GetComponentInChildren<GemCounterController>();
        gcc.AddGem();
    }
}
