using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    public class WorldTile : MonoBehaviour
    {
        public Vector3Int LocalPlace { get; set; }
    
        public Vector3Int WorldLocation { get; set; }
    
        public TileBase TileBase { get; set; }
    
        public Tilemap TilemapMember { get; set; }
    
        public string Name { get; set; }

        // Below is needed for Breadth First Searching
        public bool IsExplored { get; set; }

        public WorldTile ExploredFrom { get; set; }

        public int Cost { get; set; }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
