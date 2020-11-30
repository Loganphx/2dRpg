using System;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    public Grid grid;
    public Tilemap floor;
    public Tilemap obstacles;
    
    public Dictionary<Vector3Int, WorldTile> availablePlaces;
    public Dictionary<Vector3Int, WorldTile> obstaclePlaces;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        obstaclePlaces = new Dictionary<Vector3Int, WorldTile>();
        availablePlaces = new Dictionary<Vector3Int, WorldTile>();
        
        obstaclePlaces = BuildTilePlacesDictionary(obstacles, obstaclePlaces);
        availablePlaces = BuildTilePlacesDictionary(floor, availablePlaces);
    }


    private Dictionary<Vector3Int, WorldTile> BuildTilePlacesDictionary(Tilemap tileMap, Dictionary<Vector3Int, WorldTile> dict)
    {
        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int) 0));
                var l = floor.CellToWorld(localPlace);
                if (tileMap.HasTile(localPlace))
                {
                    //Tile at "place"
                    var tile = new WorldTile {
                        LocalPlace = localPlace,
                        WorldLocation = new Vector3Int((int) l.x, (int) l.y, 0),
                        TileBase = tileMap.GetTile(localPlace),
                        TilemapMember = tileMap,
                        Name = localPlace.x + "," + localPlace.y,
                        Cost = 1 // TODO: Change this with the proper cost from ruletile
                    };
                    // tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                    // tile.TilemapMember.SetColor(tile.LocalPlace, Color.black);
                    dict.Add(tile.WorldLocation, tile);
                }
                else
                {
                    //No tile at "place"
                }
            }
        }

        return dict;
    }
}
