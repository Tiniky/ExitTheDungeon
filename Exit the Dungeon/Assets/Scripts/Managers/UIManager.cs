using UnityEngine;
using UnityEngine.UI;

public static class UIManager {
    private static Canvas _canvas, _escapeMenuCanvas;

    public static void Initialize(){
        CreateCanvas();
        InitializeSkills();
        InitializeHealthBars();
        InitializeBasicActions();
        InitializeAbilities();
        InitializePassives();
        InitializeMinimap();
        InitializeGemCounter();
        InitializeLogs();
        InitializeCutsceneAccessories();
        InitializeFightRelatedThings();
        InitializeText();
        InitializeInventory();
        InitializeEscapeMenu();
    }

    private static void CreateCanvas(){
        GameObject canvasObject = new GameObject("Canvas_UI");
        _canvas = canvasObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject canvasObject2 = new GameObject("Canvas_EscapeMenu");
        _escapeMenuCanvas = canvasObject2.AddComponent<Canvas>();
        _escapeMenuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler2 = canvasObject2.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasObject2.AddComponent<GraphicRaycaster>();
    }

    public static GameObject _CreateEmptyUIGameObject(string name, Vector3 pos){
        GameObject go = new GameObject(name);
        go.transform.SetParent(_canvas.transform);
        go.transform.localPosition = pos;

        return go;
    }
    
    private static void InitializeSkills(){
        SkillUIManager.Initialize();
    }

    private static void InitializeHealthBars(){
        HealthBarManager.Initialize();
    }

    private static void InitializeBasicActions(){
        ActionUIManager.Initialize();
    }

    private static void InitializeAbilities(){
        AbilityUIManager.Initialize();
    }

    private static void InitializePassives(){
        PassiveUIManager.Initialize();
    }

    private static void InitializeMinimap(){
        MapManager.Initialize(_canvas);
    }

    private static void InitializeGemCounter(){
        GemUIManager.Initialize(_canvas);
    }

    private static void InitializeCutsceneAccessories(){
        CutsceneManager.Initialize(_canvas);
    }

    private static void InitializeFightRelatedThings(){
        FightUIManager.Initialize(_canvas);
    }

    private static void InitializeText(){
        TextUIManager.Initialize();
    }

    private static void InitializeInventory(){
        InventoryManager.Initialize(_canvas);
        InventoryManager.InventoryVisibility();
    }

    private static void InitializeLogs(){
        LogManager.Initialize(_canvas);
    }

    private static void InitializeEscapeMenu(){
        GameManager.InitializeEscapeMenu(_escapeMenuCanvas);
    }

    public static void UIVisibility(bool shouldBeVisible) {
        AbilityUIManager.SetVisibility(shouldBeVisible);
        ActionUIManager.SetVisibility(shouldBeVisible);
        HealthBarManager.SetVisibility(shouldBeVisible);
        MapManager.SetVisibility(shouldBeVisible);
        PassiveUIManager.SetVisibility(shouldBeVisible);
        FightUIManager.FightVisibility(shouldBeVisible);
        LogManager.SetVisibility(shouldBeVisible);
    }

    public static void HideUI(){
        _canvas.gameObject.SetActive(false);
    }

    public static void ShowUI(){
        _canvas.gameObject.SetActive(true);
    }
}
