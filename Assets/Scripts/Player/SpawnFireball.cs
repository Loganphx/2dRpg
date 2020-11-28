using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireball : MonoBehaviour
{
    public GameObject player;
    public GameObject fireball;

    private Vector3 pos;

    private GameObject clone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            spawnFireball();
        }
        
    }

    void spawnFireball()
    {
        pos = player.transform.position; 
        pos += new Vector3(1, 0);
        clone = Instantiate(fireball, pos, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().velocity += new Vector2(5.0f, 0.0f);
    }
}
