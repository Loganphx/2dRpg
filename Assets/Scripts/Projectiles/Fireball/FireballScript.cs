using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public GameObject fireball;
    public Animator animation;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            rigidBody.velocity += new Vector2(2.0f, 0.0f);
            animation.SetTrigger("shoot");

        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(fireball);
    }
    
}
