using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DungeonGeneration : MonoBehaviour
{
    [SerializeField]
    private int numberOfRooms;
    public Room CurrentRoom;
    public Room LastCreatedRoom;

    public List<GameObject> roomPrefabs = new List<GameObject>();
    public List<GameObject> treasureRoomPrefabs = new List<GameObject>();
    public List<GameObject> bossRoomPrefabs = new List<GameObject>();
 
    private List<Room> _rooms = new List<Room>();
    public Dictionary<Vector2Int, Room> dungeonMap = new Dictionary<Vector2Int, Room>();

    public CardinalDirection mainDirection;

    Vector2Int _currentPosition = new Vector2Int (0, 0);

    public GameObject roomObject;


    // Start is called before the first frame update
    void Start ()
    {
        mainDirection = CardinalDirection.North;
        GenerateDungeon();
        roomObject = (GameObject) Instantiate(dungeonMap[new Vector2Int(0, 0)].RoomPrefab);
        roomObject.SetActive(true);
        
        CurrentRoom = _rooms[0];
        GenerateRoomDoorsTwo();
    }

    public void GenerateRoomDoorsTwo()
    {
        Debug.Log($"{_rooms[0]}");
        var x = GameObject.FindObjectOfType<RoomsDoorsController>();
        
        var ds = GameObject.FindGameObjectsWithTag("Door");
        Debug.Log($"{ds.Length}");
        
        
        if (dungeonMap.ContainsKey(new Vector2Int(CurrentRoom.roomCoordinate.x + 1, CurrentRoom.roomCoordinate.y)))
        {
            
        }
        else
        {
            x.EDoor.SetActive(false);
        }

        if (dungeonMap.ContainsKey(new Vector2Int(CurrentRoom.roomCoordinate.x - 1, CurrentRoom.roomCoordinate.y)))
        {
        }
        else
        {
            x.WDoor.SetActive(false);
        }
    
        if (dungeonMap.ContainsKey(new Vector2Int(CurrentRoom.roomCoordinate.x, CurrentRoom.roomCoordinate.y - 1)))
        {
        }
        else
        {
            x.SDoor.SetActive(false);
        }
        
        if (dungeonMap.ContainsKey(new Vector2Int(CurrentRoom.roomCoordinate.x, CurrentRoom.roomCoordinate.y + 1)))
        {
        }
        else
        {
            x.NDoor.SetActive(false);
        }
        FindObjectOfType<MiniMapController>().UpdateMiniMap(dungeonMap, 
            new Vector3Int(CurrentRoom.roomCoordinate.x, CurrentRoom.roomCoordinate.y, 0));
        var oldRooms = GameObject.FindGameObjectsWithTag("Room");
        Debug.Log($"OLD ROOMS {oldRooms.Length}");
    }
 
    private void GenerateDungeon()
    {
        dungeonMap = new Dictionary<Vector2Int, Room>();
        _rooms = new List<Room>();

        Queue<Room> roomsToCreate = new Queue<Room>();
        List<Room> createdRooms = new List<Room> ();

        for (var i = 0; i < numberOfRooms; i++)
        {
            if (i == 0)
            {
                _currentPosition = new Vector2Int(0, 0);
                roomsToCreate = CreatedRooms(CardinalDirection.NoMove, roomsToCreate);
                mainDirection = CardinalDirection.North;  
            }
            else
            {
                var splitVar = Mathf.RoundToInt(Random.Range(0, 5));
                if (i % 3 == 0)
                {
                    var d = Mathf.RoundToInt(Random.Range(0, 3));
                    if (d == 1)
                    {
                        if (mainDirection == CardinalDirection.West)
                            mainDirection = CardinalDirection.West;
                        else
                            mainDirection = CardinalDirection.East;
                    }

                    if (d == 0)
                    {
                        if (mainDirection == CardinalDirection.East)
                            mainDirection = CardinalDirection.East;
                        else
                            mainDirection = CardinalDirection.West;
                    }
                
                    if (d == 2)
                    {
                        mainDirection = CardinalDirection.North;  
                    }
                }

                var currentPositionToSave = _currentPosition;
                if (splitVar == 1000)
                {
                    for (var c = 0; c < Random.Range(0, numberOfRooms); c++)
                    {
                    }
                }

                _currentPosition = currentPositionToSave;
                roomsToCreate = CreatedRooms(mainDirection, roomsToCreate);   
            }
        }

        for (var i = 0; i < numberOfRooms; i ++)
        {
            var neighbors = new List<Room>();
            Room currentRoom = roomsToCreate.Dequeue();
            Room nextRoom = null;

            if (i != 0)
            {
                neighbors.Add(LastCreatedRoom);
            }
            if (roomsToCreate.ToArray().Length > 0)
            {
                nextRoom = roomsToCreate.Peek();
                var nextsNeighborList = new List<Room>() {currentRoom};

                if (roomsToCreate.ToArray().Length >= 2)
                {
                    var nextsNeighbor = roomsToCreate.ToArray()[1];
                    nextsNeighborList.Add(nextsNeighbor);
                }
                
                var newRoomNext = new Room(nextRoom.roomCoordinate.x, nextRoom.roomCoordinate.y, roomPrefabs,
                    nextsNeighborList);
                
                neighbors.Add(newRoomNext);
            }

            Room newRoom = null;
            Debug.Log($"i: {i}");
            Debug.Log($"n: {numberOfRooms - 1}");
            if (i == numberOfRooms - 1)
            {
                newRoom = new Room(currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y, bossRoomPrefabs, neighbors);    
            }
            else
            {
                newRoom = new Room(currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y, roomPrefabs, neighbors);
            }
            
            dungeonMap[new Vector2Int(newRoom.roomCoordinate.x, newRoom.roomCoordinate.y)] =
                newRoom;

            _rooms.Add(newRoom);
            currentRoom = newRoom;
            createdRooms.Add (currentRoom);
            LastCreatedRoom = newRoom;
            // Console.WriteLine($"REEE new count{rooms.Count}");
        }


        var rRoomRange = Random.Range(1, numberOfRooms);
        var rRoom = Random.Range(1, rRoomRange);
        var n = createdRooms[rRoom].roomCoordinate;

        bool horizontal = false;
        bool upAndRight = false;
        bool upAndLeft = false;
        bool vertical = false;
        MoveDirection moveDirection = MoveDirection.None;
        
        Room rN = null;
        Room rS = null;
        Room rE = null;
        Room rW = null;
        var eP = n;
        eP.x = eP.x + 1;
        var wP = n;
        wP.x = wP.x - 1;
        var sP = n;
        sP.y = sP.y - 1;
        var nP = n;
        nP.y = nP.y + 1;

        if (dungeonMap.ContainsKey(eP))
            rE = dungeonMap[eP];
        if (dungeonMap.ContainsKey(sP))
            rS = dungeonMap[sP];
        if (dungeonMap.ContainsKey(nP))
            rN = dungeonMap[nP];
        if (dungeonMap.ContainsKey(wP))
            rW = dungeonMap[wP];

        if (rN != null && rS != null)
        {
            horizontal = true;
        }
        if (rW != null && rS != null)
        {
            upAndLeft = true;
        }
        if (rE != null && rS != null)
        {
            upAndRight = true;
        }
        if (rW != null && rE != null)
        {
            vertical = true;
        }
        
        for (int i = 0; i < rRoomRange; i++)
        {
            if (i == 0)
            {
                var splitVarLR = Random.Range(0, 1);
                if (horizontal)
                {
                    if (splitVarLR == 0)
                    {
                        _currentPosition = createdRooms[rRoom].roomCoordinate;
                        mainDirection = CardinalDirection.West;
                        roomsToCreate = CreatedRooms(CardinalDirection.West, roomsToCreate);
                        _currentPosition = roomsToCreate.Peek().roomCoordinate;
                    }
                    if (splitVarLR == 1)
                    {
                        _currentPosition = createdRooms[rRoom].roomCoordinate;
                        mainDirection = CardinalDirection.East;   
                        roomsToCreate = CreatedRooms(CardinalDirection.East, roomsToCreate);
                        _currentPosition = roomsToCreate.Peek().roomCoordinate;
                    }   
                }
                if (vertical)
                {
                    if (splitVarLR == 0)
                    {
                        _currentPosition = createdRooms[rRoom].roomCoordinate;
                        mainDirection = CardinalDirection.North;
                        roomsToCreate = CreatedRooms(CardinalDirection.North, roomsToCreate);
                        _currentPosition = roomsToCreate.Peek().roomCoordinate;
                    }
                    if (splitVarLR == 1)
                    {
                        _currentPosition = createdRooms[rRoom].roomCoordinate;
                        mainDirection = CardinalDirection.South;   
                        roomsToCreate = CreatedRooms(CardinalDirection.South, roomsToCreate);
                        _currentPosition = roomsToCreate.Peek().roomCoordinate;
                    }   
                }
                if (upAndLeft)
                {
                    if (splitVarLR == 0)
                    {
                        _currentPosition = createdRooms[rRoom].roomCoordinate;
                        mainDirection = CardinalDirection.North;
                        roomsToCreate = CreatedRooms(CardinalDirection.North, roomsToCreate);
                        _currentPosition = roomsToCreate.Peek().roomCoordinate;
                    }
                    if (splitVarLR == 1)
                    {
                        _currentPosition = createdRooms[rRoom].roomCoordinate;
                        mainDirection = CardinalDirection.East;   
                        roomsToCreate = CreatedRooms(CardinalDirection.East, roomsToCreate);
                        _currentPosition = roomsToCreate.Peek().roomCoordinate;
                    }   
                }
                if (upAndRight)
                {
                    if (splitVarLR == 0)
                    {
                        _currentPosition = createdRooms[rRoom].roomCoordinate;
                        mainDirection = CardinalDirection.North;
                        roomsToCreate = CreatedRooms(CardinalDirection.North, roomsToCreate);
                        _currentPosition = roomsToCreate.Peek().roomCoordinate;
                    }
                    if (splitVarLR == 1)
                    {
                        _currentPosition = createdRooms[rRoom].roomCoordinate;
                        mainDirection = CardinalDirection.West;   
                        roomsToCreate = CreatedRooms(CardinalDirection.West, roomsToCreate);
                        _currentPosition = roomsToCreate.Peek().roomCoordinate;
                    }   
                }
            }
            
            if (i % 2 == 0)
            {
                var d = Mathf.RoundToInt(Random.Range(0, 2));
                if (d == 1)
                {
                    if (mainDirection == CardinalDirection.West)
                        mainDirection = CardinalDirection.West;
                    else
                        mainDirection = CardinalDirection.East;
                }

                if (d == 0)
                {
                    if (mainDirection == CardinalDirection.East)
                        mainDirection = CardinalDirection.East;
                    else
                        mainDirection = CardinalDirection.West;
                }
                
                if (d == 2)
                {
                    mainDirection = CardinalDirection.North;  
                }
            }
            
            roomsToCreate = CreatedRooms(mainDirection, roomsToCreate);
            var newRoom = roomsToCreate.Peek();
            dungeonMap[_currentPosition] = newRoom;
        }

        var count = 0;
        var total = roomsToCreate.ToArray().Length;
        var ogArray = roomsToCreate.ToArray();
        foreach (var r in ogArray)
        {
            var neighbors = new List<Room>();
            Room nextRoom = null;

            if (count != 0)
            {
                neighbors.Add(LastCreatedRoom);
            }
            if (roomsToCreate.ToArray().Length > 0)
            {
                nextRoom = roomsToCreate.Peek();
                var nextsNeighborList = new List<Room>() {r};

                if (roomsToCreate.ToArray().Length >= 2)
                {
                    var nextsNeighbor = roomsToCreate.ToArray()[1];
                    nextsNeighborList.Add(nextsNeighbor);
                }

                var newRoomNext = new Room(nextRoom.roomCoordinate.x, nextRoom.roomCoordinate.y, roomPrefabs,
                    nextsNeighborList);
                
                neighbors.Add(newRoomNext);
            }

            Room newRoom = null;
            
            if (count == total - 1 || count == total)
            {
                newRoom = new Room(r.roomCoordinate.x, r.roomCoordinate.y, treasureRoomPrefabs, neighbors);
                newRoom.RoomType = RoomType.Treasure;
            }
            else
                newRoom = new Room(r.roomCoordinate.x, r.roomCoordinate.y, roomPrefabs, neighbors);

            dungeonMap[new Vector2Int(newRoom.roomCoordinate.x, newRoom.roomCoordinate.y)] =
                newRoom;

            _rooms.Add(newRoom);
            createdRooms.Add (newRoom);
            LastCreatedRoom = newRoom;
            count += 1;
            // Console.WriteLine($"REEE new count{rooms.Count}");
        }
    }

    private Queue<Room> CreatedRooms(CardinalDirection cardinalDirection, Queue<Room> roomsToCreate)
    {
        if (cardinalDirection == CardinalDirection.North)
        {
            var n = new Room(_currentPosition.x, _currentPosition.y + 1, roomPrefabs, new List<Room>() {LastCreatedRoom});
            roomsToCreate.Enqueue(n);
            _currentPosition = new Vector2Int(_currentPosition.x, _currentPosition.y + 1);
            LastCreatedRoom = n;
        }
        

        if (cardinalDirection == CardinalDirection.East)
        {
            var n = new Room(_currentPosition.x + 1, _currentPosition.y,
                roomPrefabs, new List<Room>() {LastCreatedRoom});
            roomsToCreate.Enqueue(n);
            _currentPosition = new Vector2Int(_currentPosition.x + 1, _currentPosition.y);
            LastCreatedRoom = n;
        }

        if (cardinalDirection == CardinalDirection.West)
        {
            var n = new Room(_currentPosition.x - 1, _currentPosition.y,
                roomPrefabs, new List<Room>() {LastCreatedRoom});
            roomsToCreate.Enqueue(n);
            _currentPosition = new Vector2Int(_currentPosition.x - 1, _currentPosition.y);
            LastCreatedRoom = n;
        }
        
        if (cardinalDirection == CardinalDirection.South)
        {
            var n = new Room(_currentPosition.x, _currentPosition.y - 1,
                roomPrefabs, new List<Room>() {LastCreatedRoom});
            roomsToCreate.Enqueue(n);
            _currentPosition = new Vector2Int(_currentPosition.x, _currentPosition.y - 1);
            LastCreatedRoom = n;
        }
        
        if (cardinalDirection == CardinalDirection.NoMove)
        {
            var n = new Room(_currentPosition.x, _currentPosition.y,
                roomPrefabs, new List<Room>() {LastCreatedRoom});
            roomsToCreate.Enqueue(n);
            _currentPosition = new Vector2Int(_currentPosition.x, _currentPosition.y);
            LastCreatedRoom = n;
        }

        return roomsToCreate;
    }
}

public enum CardinalDirection
{
    North,
    South,
    East,
    West,
    NoMove
}
