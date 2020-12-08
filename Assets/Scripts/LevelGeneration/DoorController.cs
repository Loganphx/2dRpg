
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Room LeadsToRoom;
    public GameObject currentRoom;
    public Transform spawnPositionX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") {
            // currentRoom.SetActive(false);
            Debug.Log($"DOORNAME: ${name}");
            var cRoomObj = FindObjectOfType<DungeonGeneration>().roomObject;
            var cRoom = FindObjectOfType<DungeonGeneration>().CurrentRoom;
            Debug.Log($"Now In Room: ${cRoom.roomCoordinate}");

            GameObject newRoomObject = null;

            if (name.ToLower().Contains("top"))
            {
                {
                    Debug.Log($"SEARCHING FOR x {cRoom.roomCoordinate.x}, y {cRoom.roomCoordinate.y + 1}");
                    var newRoomF = FindObjectOfType<DungeonGeneration>().dungeonMap
                        [new Vector2Int(cRoom.roomCoordinate.x, cRoom.roomCoordinate.y + 1)];
                    
                    var oldRooms = GameObject.FindGameObjectsWithTag("Room");
                    foreach (var r in oldRooms)
                    {
                        Destroy(r);
                    }
                    newRoomObject = (GameObject) Instantiate(newRoomF.RoomPrefab);
                    newRoomObject.SetActive(true);
                    
                    FindObjectOfType<DungeonGeneration>().roomObject = newRoomObject;  
                    FindObjectOfType<DungeonGeneration>().CurrentRoom = newRoomF;
                    
                    var newPosition = FindObjectOfType<RoomsDoorsController>().SDoor.gameObject.transform.position;
                    newPosition.y += 3;

                    GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>()
                        .transform.position = newPosition;
                           
                    FindObjectOfType<DungeonGeneration>().GenerateRoomDoorsTwo();
                }
            }
            else if (name.ToLower().Contains("bottom"))
            {
                {
                    Debug.Log("GOING DOWN");
                    var newRoomF = FindObjectOfType<DungeonGeneration>().dungeonMap
                        [new Vector2Int(cRoom.roomCoordinate.x, cRoom.roomCoordinate.y - 1)];
                    
                    var oldRooms = GameObject.FindGameObjectsWithTag("Room");
                    foreach (var r in oldRooms)
                    {
                        Destroy(r);
                    }
                    newRoomObject = (GameObject) Instantiate(newRoomF.RoomPrefab);
                    newRoomObject.SetActive(true);
                    
                    FindObjectOfType<DungeonGeneration>().roomObject = newRoomObject;
                    FindObjectOfType<DungeonGeneration>().CurrentRoom = newRoomF;
                    
                    var newPosition = FindObjectOfType<RoomsDoorsController>().NDoor.gameObject.transform.position;
                    newPosition.y -= 3;

                    GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>()
                        .transform.position = newPosition;
                    
                    FindObjectOfType<DungeonGeneration>().GenerateRoomDoorsTwo();
                }
            }
            else if (name.ToLower().Contains("left"))
            {
                Debug.Log($"GOING LEFT to {new Vector2Int(cRoom.roomCoordinate.x - 1, cRoom.roomCoordinate.y)}");
                var newRoomF = FindObjectOfType<DungeonGeneration>().dungeonMap
                    [new Vector2Int(cRoom.roomCoordinate.x - 1, cRoom.roomCoordinate.y)];

                var oldRooms = GameObject.FindGameObjectsWithTag("Room");
                foreach (var r in oldRooms)
                {
                    Destroy(r);
                }
                newRoomObject = (GameObject) Instantiate(newRoomF.RoomPrefab);
                newRoomObject.SetActive(true);

                FindObjectOfType<DungeonGeneration>().roomObject = newRoomObject;
                FindObjectOfType<DungeonGeneration>().CurrentRoom = newRoomF;
                
                var newPosition = FindObjectOfType<RoomsDoorsController>().EDoor.gameObject.transform.position;
                newPosition.x -= 3;

                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>()
                    .transform.position = newPosition;
                
                FindObjectOfType<DungeonGeneration>().GenerateRoomDoorsTwo();
            }
            else if (name.ToLower().Contains("right"))
            {
                    var newRoomF = FindObjectOfType<DungeonGeneration>().dungeonMap
                        [new Vector2Int(cRoom.roomCoordinate.x + 1, cRoom.roomCoordinate.y)];
                    
                    var oldRooms = GameObject.FindGameObjectsWithTag("Room");
                    foreach (var r in oldRooms)
                    {
                        Destroy(r);
                    }
                    
                    newRoomObject = (GameObject) Instantiate(newRoomF.RoomPrefab);
                    newRoomObject.SetActive(true);
                    
                    FindObjectOfType<DungeonGeneration>().roomObject = newRoomObject;
                    FindObjectOfType<DungeonGeneration>().CurrentRoom = newRoomF;
                    
                    
                    var newPosition = FindObjectOfType<RoomsDoorsController>().WDoor.gameObject.transform.position;
                    newPosition.x += 3;

                    GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>()
                        .transform.position = newPosition;
                    
                    FindObjectOfType<DungeonGeneration>().GenerateRoomDoorsTwo();
            }
            Debug.Log("--------------------------------------------------------");
        }
    }
}
