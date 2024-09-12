using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTemplateLayerHandler {
    public void InitializeTilemaps(GameObject obj){
        obj.AddComponent<Grid>();
        GameObject ground = CreateTilemapGameObject("Ground", obj, "Ground", 1);
        GameObject walls = CreateTilemapGameObject("Walls", obj, "Collision", 1);
        AddCompositeCollider(walls);
        GameObject shadow = CreateTilemapGameObject("Shadow", obj, "Shadow", 1);
        GameObject decoration = CreateTilemapGameObject("Decoration", obj, "GroundDecor", 1);
        GameObject front = CreateTilemapGameObject("Front", obj, "Front", 1);
        GameObject collision = CreateTilemapGameObject("Collision", obj, "Collision", 2);
        AddCompositeCollider(collision);
        GameObject minimap = CreateTilemapGameObject("Minimap", obj, "Minimap", 0);
    }

    protected GameObject CreateTilemapGameObject(string name, GameObject parentObj, string sortingLayer, int sortingOrder){
        GameObject tilemapObj = new GameObject(name);
        tilemapObj.transform.SetParent(parentObj.transform);
        Tilemap tilemap = tilemapObj.AddComponent<Tilemap>();
        TilemapRenderer renderer = tilemapObj.AddComponent<TilemapRenderer>();
        renderer.sortingLayerName = sortingLayer;
        renderer.sortingOrder = sortingOrder;
        return tilemapObj;
    }

    protected void AddCompositeCollider(GameObject obj){
        TilemapCollider2D collider = obj.AddComponent<TilemapCollider2D>();
        collider.usedByComposite = true;
        obj.AddComponent<CompositeCollider2D>();
        obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
