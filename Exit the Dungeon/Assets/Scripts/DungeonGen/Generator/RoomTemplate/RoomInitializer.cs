using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomInitializer : MonoBehaviour {
    public void Initialize(){
        RoomSettings roomSettings = gameObject.GetComponent<RoomSettings>();

        if(roomSettings == null){
            roomSettings = gameObject.AddComponent<RoomSettings>();
        }

        GameObject tilemaps = new GameObject("TileMaps");
        tilemaps.transform.parent = gameObject.transform;
        InitializeTilemaps(tilemaps);
        tilemaps.transform.localPosition = Vector3.zero;
        transform.localPosition = Vector3.zero;

        Tilemap groundTilemap = tilemaps.transform.Find("Ground").GetComponent<Tilemap>();
        roomSettings.Tilemap = groundTilemap;

        GameObject environment = new GameObject("Environment");
        environment.transform.parent = gameObject.transform;

        GameObject interactableHolder = new GameObject("InteractableTiles");
        interactableHolder.transform.parent = environment.transform;
        roomSettings.InteractableHolder = interactableHolder;
        
        GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/GameResources/Tile");
        roomSettings.TilePrefab = tilePrefab;

        InitializeComponents();
    }

    protected void InitializeTilemaps(GameObject root){
        RoomTemplateLayerHandler layersHandler = new RoomTemplateLayerHandler();
        layersHandler.InitializeTilemaps(root);
    }

    protected void InitializeComponents(){
        if(gameObject.GetComponent<Doorway>() == null){
            gameObject.AddComponent<Doorway>();
        }
    }

    #if UNITY_EDITOR
    [MenuItem("Assets/Create/SZAKDOLGOZAT/Templates/Dungeon Room")]
    public static void CreateRoomTemplatePrefab(){
        RoomTemplateInitializer.CreateRoomTemplatePrefab();
    }
    #endif
}
