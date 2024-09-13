using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(RoomSettings))]
public class RoomSettingsInspector : UnityEditor.Editor{

    private RoomSettings settings;
    
    private void OnEnable() {
        settings = (RoomSettings)target;
    }

    public override void OnInspectorGUI(){
        settings.CheckIfDoorWasUpdated();

        SerializedObject serializedSettings = new SerializedObject(settings);

        EditorGUI.BeginChangeCheck();

        SerializedProperty settingsType = serializedSettings.FindProperty("Type");
        EditorGUILayout.PropertyField(settingsType);

        EditorGUILayout.LabelField("ShouldDrawInteractableTiles (ReadOnly)", settings.ShouldDrawInteractableTiles.ToString());
        EditorGUILayout.LabelField("IsPartyMemberSpawnNeeded (ReadOnly)", settings.IsPartyMemberSpawnNeeded.ToString());
        EditorGUILayout.LabelField("AreEnemySpawnPointsNeeded (ReadOnly)", settings.AreEnemySpawnPointsNeeded.ToString());
        EditorGUILayout.LabelField("HasAtLeastOneDoor (ReadOnly)", settings.HasAtLeastOneDoor.ToString());

        SerializedProperty topLeftProp = serializedSettings.FindProperty("TopLeftCorner");
        EditorGUILayout.PropertyField(topLeftProp);
        SerializedProperty bottomRightProp = serializedSettings.FindProperty("BottomRightCorner");
        EditorGUILayout.PropertyField(bottomRightProp);

        serializedSettings.ApplyModifiedProperties();

        if(GUILayout.Button("Generate Interactable Tiles")){
            GenerateInteractableTiles();
        }

        if(GUILayout.Button("Clean Up Tiles")){

            foreach (var tile in settings.InteractablesOfRoom) {
                GameObject obj = tile.Value.gameObject;
                DestroyImmediate(obj);
            }

            settings.InteractablesOfRoom.Clear();
        }

        if(settings.Type == RoomType.NONE){
            EditorGUILayout.HelpBox("Choose a RoomType first!", MessageType.Warning);
        }

        if(EditorGUI.EndChangeCheck()){
            EditorUtility.SetDirty(settings);
            settings.TypeWasUpdated();
        }
    }

    private void GenerateInteractableTiles() {
        Tilemap tilemap = settings.GetTilemap();
        Transform holder = settings.GetHolderGameObject();
        GameObject TilePrefab = settings.GetTilePrefab();
        if (tilemap == null || TilePrefab == null) {
            Debug.LogError("Tilemap or TilePrefab is not assigned.");
            return;
        }

        Dictionary<Vector2, InteractableTile> tiles = GenerateInteractableGrid(holder, tilemap, TilePrefab);
        settings.SaveInteractableTiles(tiles);
    }

    private Dictionary<Vector2, InteractableTile> GenerateInteractableGrid(Transform holder, Tilemap tilemap, GameObject TilePrefab) {
        Dictionary<Vector2, InteractableTile> _tiles = new Dictionary<Vector2, InteractableTile>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                
                Vector3Int cellPosition = new Vector3Int(x + bounds.x, y + bounds.y, 0);
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null) {
                    GameObject TileClone = Instantiate(TilePrefab, holder);
                    TileClone.transform.position = tilemap.GetCellCenterWorld(cellPosition);
                    TileClone.name = $"Tile {cellPosition.x} {cellPosition.y}";
                    
                    InteractableTile Tile = TileClone.GetComponent<InteractableTile>();
                    bool isOffset = (Mathf.Abs(cellPosition.x) + Mathf.Abs(cellPosition.y)) % 2 == 1;
                    Tile.Initialize(isOffset);

                    _tiles[new Vector2(cellPosition.x, cellPosition.y)] = Tile;
                }
            }
        }

        return _tiles;
    }

}
