using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using System.Threading;
using DefaultNamespace.AI.Models;
using DefaultNamespace.AI.PathFinding;
using DefaultNamespace.Helpers;
using Helpers;
using Ludiq;
using Tiles;
using UnityEngine.Internal;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class PathFindingController : MonoBehaviour
{
    public GameObject targetLocation;

    public WorldTile tile;

    private bool _awake;
    
    public int backTrackInc = 1;

    public PathFindingResponse pathFindingResponse;
    private TileMapController tileMapController;

    protected int frames = 0;
    public Coroutine currentRoutine;
    public GameObject[] potentialTargets;
    
    [SerializeField] private float AiSpeed;

    [Range(1, 100)]
    public float stopDistance = 1; 
    
    [Range(1, 100)]
    public float seekingDistance = 0; 

    // [SerializeField] private PathFindingResponse pathFindingResponse;

    // Start is called before the first frame update
    public virtual void Start()
    {
        tileMapController = FindObjectOfType<TileMapController>();
        
        potentialTargets = new GameObject[] { };
        potentialTargets = GameObject.FindGameObjectsWithTag("Player");
        
        _awake = false;
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (targetLocation != null)
        {
            var d = Vector3.Distance(transform.position, targetLocation.transform.position);
            if (d <= stopDistance)
            {
                if (currentRoutine != null)
                {
                    StopCoroutine(currentRoutine);
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;    
                }
            }   
        }

        frames++;
        if (frames % 300 == 0) { //If the remainder of the current frame divided by 10 is 0 run the function.

            float? lowestDistance = null;
            int lowestIndex = 0;
            int index = 0;
            foreach (var target in potentialTargets)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (lowestDistance == null)
                {
                    lowestDistance = distance;
                    lowestIndex = index;
                }

                if (distance < lowestDistance)
                {
                    lowestDistance = distance;
                    lowestIndex = index;
                }

                index += 1;
            }

            if (lowestDistance >= stopDistance && lowestDistance <= seekingDistance)
            {
                targetLocation = potentialTargets[lowestIndex];
                CalculatePathAndStartMovementCoroutine();
            }
            else
            {
                if (currentRoutine != null)
                    StopCoroutine(currentRoutine);
            }
            frames = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
            CleanUptrails();
    }


    private IEnumerator NewDelay()
    {
        yield return new WaitForSeconds(.05f);
        currentRoutine = StartCoroutine(MoveSprite(pathFindingResponse.NodeHistory));
    }

    private IEnumerator MoveSprite(List<WorldTile> finalNodePath)
    {
        WaitForSeconds wait = new WaitForSeconds( .15f ) ;
        foreach (WorldTile node in finalNodePath)
        {
            var moveDirectionNeeded = XYToMoveDirectionHelper.XYToMoveDirection(
                new Vector2Int((int) transform.position.x, (int) transform.position.y),
                new Vector2Int(node.LocalPlace.x, node.LocalPlace.y));

            var vectorVelocity = new MoveDirectionToVelocity(moveDirectionNeeded).Velocity;
            Debug.Log(vectorVelocity);

            if (vectorVelocity != Vector2.zero)
                GetComponent<Rigidbody2D>().velocity = vectorVelocity * AiSpeed;
                
            if (vectorVelocity == Vector2.zero)
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            // transform.position = node.LocalPlace;
            
            yield return wait;
        }

        if (Vector3.Distance(transform.position, targetLocation.transform.position) > 2)
        {
            CalculatePathAndStartMovementCoroutine();
        }
    }
    
    public void CalculatePathAndStartMovementCoroutine()
    {
        var pointa = new Vector3Int((int) transform.position.x, (int) transform.position.y, 0);
        var pointb = new Vector3Int((int) targetLocation.transform.position.x, (int) targetLocation.transform.position.y, 0);
        if (currentRoutine != null)
        {
            backTrackInc = 1;
            StopCoroutine(currentRoutine);   
        }
        try
        {
            pathFindingResponse = CalculatePath(pointa, pointb);
            StartCoroutine(NewDelay());
        }
        catch
        {
            frames = 0;
            backTrackInc = 1;
            if (currentRoutine != null)
            {
                backTrackInc = 1;
                StopCoroutine(currentRoutine);   
            }
            // ignored
        }

        // Debug.Log(JsonUtility.ToJson(pathFindingResponse.));
    }
    
    
    

    public PathFindingResponse CalculatePath(Vector3Int pointA, Vector3Int pointB)
    {
        var pathFindingResponse = new PathFindingResponse();
        pathFindingResponse.VisitedTiles = new Dictionary<Vector3Int, WorldTile>();
        pathFindingResponse.PathFindingStateList.Add(AIState.Idle);
        pathFindingResponse.CurrentPoint = pointA;
        pathFindingResponse.PointA = pointA;
        pathFindingResponse.PointB = pointB;
        

        while (pathFindingResponse.PathFindingStateList[pathFindingResponse.PathFindingStateList.Count - 1] !=
               AIState.TargetReached)
        {
            pathFindingResponse = MoveNodes(pathFindingResponse);
            if (pathFindingResponse.PathFindingStateList[pathFindingResponse.PathFindingStateList.Count - 1] != AIState.BackTracking)
                pathFindingResponse.CurrentPoint = pathFindingResponse.NodeHistory[pathFindingResponse.NodeHistory.Count - 1].LocalPlace;
            else
                pathFindingResponse.CurrentPoint = pathFindingResponse.NodeHistory[pathFindingResponse.NodeHistory.Count - backTrackInc].LocalPlace;
        }

        return pathFindingResponse;
    }
    
    PathFindingResponse MoveNodes(PathFindingResponse pathFindingResponse)
    {
        if (pathFindingResponse.CurrentPoint == pathFindingResponse.PointB && pathFindingResponse.PathFindingStateList[pathFindingResponse.PathFindingStateList.Count - 1] != AIState.TargetReached)
            pathFindingResponse.PathFindingStateList.Add(AIState.TargetReached);
            
        if (pathFindingResponse.PathFindingStateList[pathFindingResponse.PathFindingStateList.Count - 1] != AIState.TargetReached)
            pathFindingResponse = EvaluateMoveCost(pathFindingResponse);

        return pathFindingResponse;
    }
    
    
    PathFindingResponse EvaluateMoveCost(PathFindingResponse pathFindingResponse)
    {
        var nextMoveCost = new MoveCost(pathFindingResponse.CurrentPoint, pathFindingResponse.PointB,
            tileMapController.obstaclePlaces, tileMapController.availablePlaces, pathFindingResponse.VisitedTiles);

        pathFindingResponse = TranslateBestMoveToNewPosition(EvaluateBestMove(nextMoveCost), pathFindingResponse);

        return pathFindingResponse;
    }
    
    
    PathFindingResponse TranslateBestMoveToNewPosition(MoveNames bestMove, PathFindingResponse pathFindingResponse)
    {
        var key = $"";
        Vector3Int newPosition = new Vector3Int();
        Vector3Int worldPoint = new Vector3Int();
        
        var s = pathFindingResponse.CurrentPoint;
        switch (bestMove)
        {
            case MoveNames.Up:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = new Vector3Int((int) s.x, (int) (s.y + 1), (int) 0);
                key = $"{s.x}-{s.y + 1}-{0}";
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                pathFindingResponse.NodeHistory.Add(tileMapController.availablePlaces[newPosition]);
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}
                
                if (backTrackInc > 1)
                {
                    pathFindingResponse.NodeHistory.RemoveRange(pathFindingResponse.NodeHistory.Count - backTrackInc, backTrackInc);
                }
                backTrackInc = 1;
                return pathFindingResponse;
                break;
            // return new Vector3(s.x, s.y + 1, 0);
                // return Vector3.up;
            case MoveNames.UpRight:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = new Vector3Int((int) s.x + 1, (int) (s.y + 1), (int) 0);
                key = $"{s.x + 1}-{s.y + 1}-{0}";
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                pathFindingResponse.NodeHistory.Add(tileMapController.availablePlaces[newPosition]);
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}
                if (backTrackInc > 1)
                {
                    pathFindingResponse.NodeHistory.RemoveRange(pathFindingResponse.NodeHistory.Count - backTrackInc, backTrackInc);
                }
                backTrackInc = 1;
                return pathFindingResponse;
                break;
            // return new Vector3(s.x +1, s.y + 1, 0);
                // return Vector3.up + Vector3.right;
            case MoveNames.UpLeft:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = new Vector3Int((int) s.x - 1, (int) (s.y + 1), (int) 0);
                key = $"{s.x - 1}-{s.y + 1}-{0}";
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                pathFindingResponse.NodeHistory.Add(tileMapController.availablePlaces[newPosition]);
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}
                
                if (backTrackInc > 1)
                {
                    pathFindingResponse.NodeHistory.RemoveRange(pathFindingResponse.NodeHistory.Count - backTrackInc, backTrackInc);
                }
                backTrackInc = 1;
                return pathFindingResponse;
                break;
            // return new Vector3(s.x - 1, s.y + 1, 0);
                // return Vector3.up + Vector3.left;
            case MoveNames.Right:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = new Vector3Int((int) s.x + 1, (int) (s.y), (int) 0);
                key = $"{s.x + 1}-{s.y}-{0}";
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                pathFindingResponse.NodeHistory.Add(tileMapController.availablePlaces[newPosition]);
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}
                
                if (backTrackInc > 1)
                {
                    pathFindingResponse.NodeHistory.RemoveRange(pathFindingResponse.NodeHistory.Count - backTrackInc, backTrackInc);
                }
                backTrackInc = 1;
                return pathFindingResponse;
                break;
            // return new Vector3(s.x + 1, s.y, 0);
                // return Vector3.right;
            case MoveNames.Left:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = new Vector3Int((int) s.x - 1, (int) (s.y), (int) 0);
                key = $"{s.x - 1}-{s.y}-{0}";
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                pathFindingResponse.NodeHistory.Add(tileMapController.availablePlaces[newPosition]);
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}
                
                if (backTrackInc > 1)
                {
                    pathFindingResponse.NodeHistory.RemoveRange(pathFindingResponse.NodeHistory.Count - backTrackInc, backTrackInc);
                }
                backTrackInc = 1;
                return pathFindingResponse;
                break;
            //return new Vector3(s.x - 1, s.y, 0);
                // return Vector3.left;
            case MoveNames.Down:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = new Vector3Int((int) s.x, (int) (s.y - 1), (int) 0);
                key = $"{s.x}-{s.y - 1}-{0}";
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                pathFindingResponse.NodeHistory.Add(tileMapController.availablePlaces[newPosition]);
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}
                
                if (backTrackInc > 1)
                {
                    pathFindingResponse.NodeHistory.RemoveRange(pathFindingResponse.NodeHistory.Count - backTrackInc, backTrackInc);
                }
                backTrackInc = 1;
                return pathFindingResponse;
                break;
            // return new Vector3(s.x, s.y - 1, 0);
                // return Vector3.down;
            case MoveNames.DownRight:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = new Vector3Int((int) s.x + 1, (int) (s.y - 1), (int) 0);
                key = $"{s.x + 1}-{s.y - 1}-{0}";
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                pathFindingResponse.NodeHistory.Add(tileMapController.availablePlaces[newPosition]);
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}
                
                if (backTrackInc > 1)
                {
                    pathFindingResponse.NodeHistory.RemoveRange(pathFindingResponse.NodeHistory.Count - backTrackInc, backTrackInc);
                }
                backTrackInc = 1;
                return pathFindingResponse;
                break;
            // return new Vector3(s.x + 1, s.y - 1, 0);
                // return Vector3.down + Vector3.right;
            case MoveNames.DownLeft:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = new Vector3Int((int) s.x - 1, (int) (s.y - 1), (int) 0);
                key = $"{s.x - 1}-{s.y - 1}-{0}";
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                pathFindingResponse.NodeHistory.Add(tileMapController.availablePlaces[newPosition]);
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}

                if (backTrackInc > 1)
                {
                    pathFindingResponse.NodeHistory.RemoveRange(pathFindingResponse.NodeHistory.Count - backTrackInc,backTrackInc);
                }
                backTrackInc = 1;
                return pathFindingResponse;
                break;
            // return new Vector3(s.x - 1, s.y - 1, 0);
                // return Vector3.down + Vector3.left;
            case MoveNames.BackTrack:
                pathFindingResponse.PathFindingStateList.Add(AIState.SeekingTarget);
                newPosition = pathFindingResponse.NodeHistory[pathFindingResponse.NodeHistory.Count - backTrackInc].LocalPlace;
                worldPoint = new Vector3Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y), 0);
                //if (tileMapController.availablePlaces.TryGetValue(worldPoint, out tile)) 
                //{
                //   print("Tile " + tile.Name + " costs: " + tile.Cost);
                //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
                //    tile.TilemapMember.SetColor(tile.LocalPlace, bestMove == MoveNames.BackTrack ? Color.red : Color.green);
                //}
                
                if (pathFindingResponse.VisitedTiles.ContainsKey(newPosition) == false)
                    pathFindingResponse.VisitedTiles.Add(newPosition, tileMapController.availablePlaces[newPosition]);
                
                pathFindingResponse.PathFindingStateList.Add(AIState.BackTracking);
                    
                backTrackInc += 1;
                break;
                
            default:
                // return new Vector3(s.x, s.y, 0);
                break;
        }
        
        return pathFindingResponse;
    }

    
    MoveNames EvaluateBestMove(MoveCost moveCost)
    {
        var availableMoves = new Dictionary<MoveNames, MoveCostAndH>();
        if (Math.Abs(moveCost.Up - (-1)) > .1) {availableMoves.Add(MoveNames.Up, new MoveCostAndH(moveCost.Up, moveCost.UpH));}
        if (Math.Abs(moveCost.UpRight - (-1)) > .1) {availableMoves.Add(MoveNames.UpRight, new MoveCostAndH(moveCost.UpRight, moveCost.UpRightH));}
        if (Math.Abs(moveCost.UpLeft - (-1)) > .1) {availableMoves.Add(MoveNames.UpLeft, new MoveCostAndH(moveCost.UpLeft, moveCost.UpLeftH));}
        
        if (Math.Abs(moveCost.Right - (-1)) > .1) {availableMoves.Add(MoveNames.Right, new MoveCostAndH(moveCost.Right, moveCost.RightH));}
        if (Math.Abs(moveCost.Left - (-1)) > .1) {availableMoves.Add(MoveNames.Left, new MoveCostAndH(moveCost.Left, moveCost.LeftH));}
        
        if (Math.Abs(moveCost.DownRight - (-1)) > .1) {availableMoves.Add(MoveNames.DownRight, new MoveCostAndH(moveCost.DownRight, moveCost.DownRightH));}
        if (Math.Abs(moveCost.Down - (-1)) > .1) {availableMoves.Add(MoveNames.Down, new MoveCostAndH(moveCost.Down, moveCost.DownH));}
        if (Math.Abs(moveCost.DownLeft - (-1)) > .1) {availableMoves.Add(MoveNames.DownLeft, new MoveCostAndH(moveCost.DownLeft, moveCost.DownLeftH));}
        var val = availableMoves.OrderBy(k => k.Value.H).FirstOrDefault();
        if (availableMoves.Count == 0)
        {
            return MoveNames.BackTrack;
        }
        
        return val.Key;
    }

    void CleanUptrails()
    {
        foreach (var vec in tileMapController.availablePlaces)
        {
            vec.Value.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
            vec.Value.TilemapMember.SetColor(tile.LocalPlace, Color.clear);
        }
    }
}
