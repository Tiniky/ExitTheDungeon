using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager {
    public static TileManager Instance { get; private set; }

    private int _width, _height;
    private float _startX, _startY;
    public GameObject TilePrefab;
    private Dictionary<Vector2, InteractableTile> _tiles;
    private List<InteractableTile> _tilesInRange;
    public bool shouldRepaint;

    static TileManager(){
        Instance = new TileManager();
    }

    public void Initialize(){
        Instance = this;
        _tilesInRange = new List<InteractableTile>();
        _tiles = new Dictionary<Vector2, InteractableTile>();
        Debug.Log("TileManager initialized.");
    }

    public void LoadCurrentRoom(InstantiatedRoom room){
        _tiles.Clear();
        if(room == null){
            Debug.LogError("No room found.");
            return;
        }

        Dictionary<Vector2, InteractableTile> tiles = room.GetInteractables();
        
        if(tiles == null) {
            Debug.LogError("No interactable tiles found in CurrentRoom.");
            return;
        }
    
        foreach(var tile in tiles){
            tile.Value.Initialize();
            _tiles.Add(tile.Key, tile.Value);
        }
    }

    private InteractableTile GetTileAtPosition(Vector2 pos){
        if(_tiles.TryGetValue(pos, out InteractableTile tile)){
            return tile;
        }

        return null;
    }

    private Vector2 GetPosOfTile(InteractableTile tile){
        foreach(var tilePos in _tiles){
            if(tilePos.Value == tile){
                return tilePos.Key;
            }
        }

        return Vector2.zero;
    }

    public void SnapToClosestTile(GameObject fighter, InteractableTile tile = null){
        Vector2 closestTile;
        
        if(tile == null){
            closestTile = GetClosestTilePosition(fighter.transform.position);
            Debug.Log("TM - closest tile is " + closestTile);
            tile = GetTileAtPosition(closestTile);
            Debug.Log("TM - tile is " + tile.gameObject.name);
        } else {
            closestTile = GetPosOfTile(tile);
            Debug.Log("TM - tile is " + tile.gameObject.name);
        }

        if(tile != null){
            if(fighter.GetComponent<Entity>().Size == Size.MEDIUM){
                tile.TileOccupation(fighter);
                fighter.transform.position = tile.transform.position + new Vector3(0f,0.6f,0f);
            } else if(fighter.GetComponent<Entity>().Size == Size.LARGE){
                InteractableTile tile2 = GetTileAtPosition(closestTile + new Vector2(-1f, 0f));
                tile.TileOccupation(fighter);
                tile2.TileOccupation(fighter);
                fighter.transform.position = tile.transform.position + new Vector3(-0.5f, 1f, 0f);
            } else if(fighter.GetComponent<Entity>().Size == Size.SMALL){
                tile.TileOccupation(fighter);
                fighter.transform.position = tile.transform.position + new Vector3(0f,0.3f,0f);
            }
        }
    }

    private Vector2 GetClosestTilePosition(Vector3 position){
        float smallestDistance = 10f;
        Vector2 closestTile = Vector2.zero;

        foreach(var tilePos in _tiles.Keys){
            float distance = Vector3.Distance(position, tilePos);

            if(distance < smallestDistance){
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

    public InteractableTile GetClosestEmptyTile(Vector3 position){
        Vector2 closestDistance = GetClosestTilePosition(position);
        InteractableTile tile = GetTileAtPosition(closestDistance);

        if(tile.isEmpty){
            return tile;
        } else {
            return GetClosestEmptyTile(position + new Vector3(0.5f, 0f, 0f));
        }
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

    public List<InteractableTile> StandsOn(GameObject entity, int count){
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

    public void Reset(){
        foreach(InteractableTile tile in _tilesInRange){
            tile.ResetColor();
        }

        _tilesInRange.Clear();
    }

    public bool IsTwoEntityInRange(Entity from, Entity target, int range){
        List<InteractableTile> tiles = new List<InteractableTile>();

        if(target.Size == Size.MEDIUM){
            tiles = StandsOn(target.gameObject, 1);
            if(tiles.Count > 0){
                return tiles[0].IsTileInEntityRange(from.gameObject, range);
            } else {
                return false;
            }
        } else {
            tiles = StandsOn(target.gameObject, 2);
             if(tiles.Count > 0){
                InteractableTile closest = GetClosestFrom(tiles, from.gameObject);
                return closest.IsTileInEntityRange(from.gameObject, range);
            } else {
                return false;
            }
        }
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

    public void ReleaseTiles(){
        List<Entity> entities = new List<Entity>();
        entities.AddRange(BattleManager.GetStillAlive());

        foreach(Entity entity in entities){
            FreeTiles(entity);
        }
    }

    private List<InteractableTile> GetNeighborTiles(InteractableTile tile){
        List<InteractableTile> neighbors = new List<InteractableTile>();
        Vector2 tilePos = GetPosOfTile(tile);

        foreach(var tilePosPair in _tiles){
            Vector2 neighborPos = tilePosPair.Key;
            if(Mathf.Abs(tilePos.x - neighborPos.x) + Mathf.Abs(tilePos.y - neighborPos.y) == 1){
                neighbors.Add(tilePosPair.Value);
            }
        }

        Debug.Log("TM - Neighbors of " + tile.gameObject.name + " are " + neighbors.Count);
        return neighbors;
    }

    public InteractableTile GetClosestNeighbor(InteractableTile tile, GameObject entity){
        List<InteractableTile> neighbors = GetNeighborTiles(tile);
        InteractableTile closest = GetClosestFrom(neighbors, entity);
        return closest;
    }
}
