using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace DefaultNamespace.AI.Models
{
    public class PathFindingResponse
    {
        public Vector3Int CurrentPoint;
        public Vector3Int PointA;
        public Vector3Int PointB;
    
        public List<AIState> PathFindingStateList = new List<AIState>() {AIState.Idle};
        public List<WorldTile> NodeHistory = new List<WorldTile>();
        // public List<WorldTile> FinalNodePath = new List<WorldTile>();
    
        public Dictionary<Vector3Int, WorldTile> VisitedTiles = new Dictionary<Vector3Int, WorldTile>();
    }
}