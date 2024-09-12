using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CutsceneManager {
    private static GameObject _cutscenePrefab, _cutsceneVoid;
    private static VoidController _void;

    public static void Initialize(Canvas canvas){
        _cutscenePrefab = PrefabManager.CUTSCENE;
        _cutsceneVoid = PrefabManager.InstantiatePrefabV2(_cutscenePrefab, canvas.gameObject, false, new Vector3(0f,0f,0f), "Cutscene Accessories");
        _void = _cutsceneVoid.GetComponent<VoidController>();
    }

    public static void SetUpHeights(int top, int bottom) {
        RectTransform topCover = GameObject.Find("TopVoid").GetComponent<RectTransform>();
        RectTransform bottomCover = GameObject.Find("BottomVoid").GetComponent<RectTransform>();

        Vector2 topCoverOffsetMin = new Vector2(0, bottom);
        Vector2 topCoverOffsetMax = new Vector2(0, 0);
        topCover.offsetMin = topCoverOffsetMin;
        topCover.offsetMax = topCoverOffsetMax;

        Vector2 bottomCoverOffsetMin = new Vector2(0, 0);
        Vector2 bottomCoverOffsetMax = new Vector2(0, top * (-1));
        bottomCover.offsetMin = bottomCoverOffsetMin;
        bottomCover.offsetMax = bottomCoverOffsetMax;
    }

    public static void StartUp() {
        _void.StartCutscene();
    }

    public static void WrapUp() {
        _void.EndCutscene();
    }
}
