using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Room
{
    public Vector2Int roomCoordinate;
    public GameObject RoomPrefab;
    public RoomType RoomType;

    public List<Room> Neighbors = new List<Room>();

    public bool NorthDoor;
    public bool SouthDoor;
    public bool EastDoor;
    public bool WestDoor;

    public bool Explored;

    public Room(int xCoordinate, int yCoordinate, List<GameObject> roomPrefabs, List<Room> lastCreatedRoom)
    {
        roomCoordinate = new Vector2Int(xCoordinate, yCoordinate);
        if (lastCreatedRoom == null)
        {
            Neighbors.Add(null);
        }
        else
        {
            Neighbors =lastCreatedRoom;
        }
        RoomPrefab = SelectRandomRoom(roomPrefabs);
        RoomType = RoomType.Normal;
        Explored = false;
    }

    public Room(Vector2Int roomCoordinate, List<GameObject> roomPrefabs, List<Room> lastCreatedRoom)
    {
        this.roomCoordinate = roomCoordinate;
        Neighbors = lastCreatedRoom;
        RoomPrefab = SelectRandomRoom(roomPrefabs);
        RoomType = RoomType.Normal;
        Explored = false;
    }

    private GameObject SelectRandomRoom(List<GameObject> roomPrefabs)
    {
        var i = Mathf.RoundToInt(Random.Range(0, roomPrefabs.Count));
        return roomPrefabs[i];
    }
}

public enum RoomType
{
    Normal,
    Treasure,
    Boss
}