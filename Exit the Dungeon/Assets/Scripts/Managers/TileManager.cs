using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour {
    public static TileManager instance;

    private int _width, _height;
    private float _startX, _startY;
    public GameObject TilePrefab;
    private Dictionary<Vector2, InteractableTile> _tiles;
    private List<InteractableTile> _tilesInRange;
    public bool shouldRepaint;

    void Awake(){
        instance = this;
        _tilesInRange = new List<InteractableTile>();
    }

    public Dictionary<Vector2, InteractableTile> GenerateInteractableGrid(GameObject holder, int _width, int _height, float _startX, float _startY) {
        _tiles = new Dictionary<Vector2, InteractableTile>();

        for(int x = 0; x < _width; x++){
            for(int y = 0; y < _height; y++){
                GameObject TileClone = Instantiate(TilePrefab, holder.transform);
                TileClone.transform.position = new Vector3(_startX + x, _startY + y, 0);
                TileClone.name = $"Tile {_startX + x} {_startY + y}";
                InteractableTile Tile = TileClone.GetComponent<InteractableTile>();
                var isOffset = (x + y) % 2 == 1;
                Tile.Initialize(isOffset);

                _tiles[new Vector2(_startX + x, _startY + y)] = Tile;
            }
        }

        return _tiles;
    }

    private InteractableTile GetTileAtPosition(Vector2 pos) {
        if(_tiles.TryGetValue(pos, out InteractableTile tile)){
            return tile;
        }

        return null;
    }

    public void SnapToClosestTile(GameObject fighter) {
        Vector2 closestTile = GetClosestTilePosition(fighter.transform.position);
        InteractableTile tile = GetTileAtPosition(closestTile);

        if (tile != null){
            if(fighter.GetComponent<Entity>().Size == Size.MEDIUM){
                tile.TileOccupation(fighter);
                if(fighter == GameManager.PlayerObj()){
                    fighter.transform.position = tile.transform.position + new Vector3(0f,0.5f,0f);
                } else {
                    fighter.transform.position = tile.transform.position + new Vector3(0f,0.3f,0f);
                }
            } else if(fighter.GetComponent<Entity>().Size == Size.LARGE){
                InteractableTile tile2 = GetTileAtPosition(closestTile + new Vector2(-1f, 0f));
                tile.TileOccupation(fighter);
                tile2.TileOccupation(fighter);
                fighter.transform.position = tile.transform.position + new Vector3(-0.5f, 1.3f, 0f);
            } else if(fighter.GetComponent<Entity>().Size == Size.SMALL){
                tile.TileOccupation(fighter);
                fighter.transform.position = tile.transform.position + new Vector3(0f,0.3f,0f);
            }
        }
    }

    private Vector2 GetClosestTilePosition(Vector3 position) {
        float smallestDistance = 10f;
        Vector2 closestTile = Vector2.zero;

        foreach (var tilePos in _tiles.Keys){
            float distance = Vector3.Distance(position, tilePos);

            if (distance < smallestDistance){
                smallestDistance = distance;
                closestTile = new Vector2(tilePos.x, tilePos.y);
            }
        }

        return closestTile;
    }

    public InteractableTile GetClosestTile(Vector3 position){
        Vector2 closestDistance = GetClosestTilePosition(position);
        return GetTileAtPosition(closestDistance);
    }

    public InteractableTile GetClosestFrom(List<InteractableTile> tiles, GameObject from){
        float minDistance = Mathf.Infinity;
        InteractableTile closestTile = null;

        foreach(InteractableTile tile in tiles) {
            int distance = tile.DistanceFromEntity(from);
            if (distance < minDistance) {
                minDistance = distance;
                closestTile = tile;
            }
        }

        return closestTile;
    }

    public List<InteractableTile> StandsOn(GameObject entity, int count) {
        return _tiles.Values.Where(e => !e.isEmpty && e.EntityOnTile() == entity).Take(count).ToList();
    }

    public void ShowWalkRange(Entity entity){
        Reset();

        if(shouldRepaint){
            foreach(var tilePairs in _tiles){
                Vector2 tilePos = tilePairs.Key;
                Vector2 entityPos = entity.gameObject.transform.position - new Vector3(0f,0.5f,0f);
                int walkingRadius = entity.Speed.StepsLeft();
                float distance = Mathf.Abs(tilePos.x - entityPos.x) + Mathf.Abs(tilePos.y - entityPos.y);
                if(distance <= walkingRadius){
                    _tilesInRange.Add(tilePairs.Value);
                    tilePairs.Value.PaintTile(true);
                } else if(distance > walkingRadius && distance <= walkingRadius + 1){
                    _tilesInRange.Add(tilePairs.Value);
                    tilePairs.Value.PaintTile(false);
                }
            }
        }
    }

    public void Reset() {
        foreach(InteractableTile tile in _tilesInRange){
            tile.ResetColor();
        }

        _tilesInRange.Clear();
    }

    public bool isTwoEntityInRange(Entity from, Entity target, int range){
        List<InteractableTile> tiles = new List<InteractableTile>();

        if(target.Size == Size.MEDIUM){
            tiles = StandsOn(target.gameObject, 1);
            return tiles[0].IsTileInEntityRange(from.gameObject, range);
        } else {
            tiles = StandsOn(target.gameObject, 2);
            InteractableTile closest = GetClosestFrom(tiles, from.gameObject);
            return closest.IsTileInEntityRange(from.gameObject, range);
        }
    }

    public void IgniteTile(Vector3 position){
        InteractableTile tile = GetClosestTile(position);
        tile.isFire = true;
    }

    public void FreeTiles(Entity entity){
        List<InteractableTile> tiles = new List<InteractableTile>();

        if(entity.Size == Size.MEDIUM){
            tiles = StandsOn(entity.gameObject, 1);
        } else {
            tiles = StandsOn(entity.gameObject, 2);
        }

        foreach(InteractableTile tile in tiles){
            tile.TileOccupation();
        }
    }
}
