using System;
using System.Collections.Generic;
using DefaultNamespace;
using Scripts.Gameplay;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniMapController : MonoBehaviour
{
    public Grid miniMapGrid;
    public Tilemap miniMapTileMap;
    public Tilemap miniMapIconsTileMap;
    
    public Dictionary<Vector3Int, WorldTile> availablePlaces;
    public Dictionary<Vector3Int, WorldTile> availablePlacesIcons;
    public Dictionary<Vector3Int, WorldTile> obstaclePlaces;

    public GameObject miniMapCamera;

    public Vector3Int LastPosition;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        availablePlaces = new Dictionary<Vector3Int, WorldTile>();
        availablePlacesIcons = new Dictionary<Vector3Int, WorldTile>();
        availablePlaces = BuildTilePlacesDictionary(miniMapTileMap, availablePlaces);
        availablePlacesIcons = BuildTilePlacesDictionary(miniMapIconsTileMap, availablePlacesIcons);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMiniMap(Dictionary<Vector2Int, Room> dungeonMap, Vector3Int currentPosition)
    {
        Debug.Log("WOW");
        Debug.Log($"WOW MM {availablePlaces.Count}");
        Debug.Log($"WOW MMI {availablePlacesIcons.Count}");
        
        Debug.Log(dungeonMap.Keys);
        Debug.Log(string.Join(", ", dungeonMap.Keys));
        Debug.Log(string.Join(", ", availablePlaces.Keys));
        foreach (var kv in dungeonMap)
        {
            var tile = availablePlaces[new Vector3Int(kv.Value.roomCoordinate.x - 95, kv.Value.roomCoordinate.y + 200, 0)];
            //   print("Tile " + tile.Name + " costs: " + tile.Cost);
            // if (kv.Value.Explored)
            {
                tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                tile.TilemapMember.SetColor(tile.LocalPlace, Color.green);
                if (currentPosition.x == kv.Key.x && currentPosition.y == kv.Key.y)
                {
                    tile.TilemapMember.SetColor(tile.LocalPlace, Color.red);
                    if (LastPosition.x - currentPosition.x == +1)
                    {
                        miniMapCamera.GetComponent<CameraController>().MoveMiniMapCamera(MoveDirection.Left);
                    }
                    if (LastPosition.y - currentPosition.y == -1)
                    {
                        miniMapCamera.GetComponent<CameraController>().MoveMiniMapCamera(MoveDirection.Up);
                    }
                    if (LastPosition.x - currentPosition.x == -1)
                    {
                        miniMapCamera.GetComponent<CameraController>().MoveMiniMapCamera(MoveDirection.Right);
                    }
                    if (LastPosition.y - currentPosition.y == 1)
                    {
                        miniMapCamera.GetComponent<CameraController>().MoveMiniMapCamera(MoveDirection.Down);
                    }
                    
                    LastPosition = currentPosition;
                }
            }
            //}
            
            // miniMapTileMap.RefreshTile(new Vector3Int(kv.Value.roomCoordinate.x, kv.Value.roomCoordinate.y, 0));
        }
        Debug.Log($"LAST POSITION: {LastPosition}");
        Debug.Log($"CURRENT POSITION: {currentPosition}");
        
    }
    
    private Dictionary<Vector3Int, WorldTile> BuildTilePlacesDictionary(Tilemap tileMap, Dictionary<Vector3Int, WorldTile> dct)
    {
        dct = new Dictionary<Vector3Int, WorldTile>();
        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int) 0));
                var l = tileMap.CellToWorld(localPlace);
                if (dct.ContainsKey(new Vector3Int((int) l.x, (int) l.y, (int) l.z)) == false)
                {
                    //Tile at "place"
                    var tile = new WorldTile
                    {
                        LocalPlace = localPlace,
                        WorldLocation = new Vector3Int((int) l.x, (int) l.y, 0),
                        TileBase = tileMap.GetTile(localPlace),
                        TilemapMember = tileMap,
                        Name = localPlace.x + "," + localPlace.y,
                        Cost = 1 // TODO: Change this with the proper cost from ruletile
                    };
                    // tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                    // tile.TilemapMember.SetColor(tile.LocalPlace, Color.black);
                    dct.Add(tile.WorldLocation, tile);
                }
            }
        }

        return dct;
    }

    private void MoveMiniMapCamera(MoveDirection moveDirection)
    {
        Debug.Log("MOVE CAMERA?");
        var position = transform.position;

        if (moveDirection == MoveDirection.Down)
        {
            position = Vector3.Lerp(
                position,
                new Vector3(position.x, position.y - 1, position.z),
                Time.deltaTime * 5
            );
        }
        if (moveDirection == MoveDirection.Left){
            position = Vector3.Lerp(
                position,
                new Vector3(position.x - 1, position.y, position.z),
                Time.deltaTime * 5
            );}
        if (moveDirection == MoveDirection.Right){
            position = Vector3.Lerp(
                position,
                new Vector3(position.x + 1, position.y, position.z),
                Time.deltaTime * 5
            );}
        if (moveDirection == MoveDirection.Up){
            position = Vector3.Lerp(
                position,
                new Vector3(position.x, position.y + 1, position.z),
                Time.deltaTime * 5
            );}

        transform.position = position;
    }
}
