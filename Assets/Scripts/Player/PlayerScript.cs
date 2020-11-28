using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.XR;

public class PlayerScript : MonoBehaviour
{
    private const float SPEED = 10f;
    bool facingRight = true;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();    
    }

    private void HandleMovement()
    {
        
        ;
        float moveX = 0f;
        float moveY = 0f;

        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 theScale = transform.localScale;
            if(facingRight == true)
            {
            theScale.x *= -1;
            transform.localScale = theScale;
            facingRight = false;
            }
            moveX = -1f;

        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
            
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 theScale = transform.localScale;
            if (facingRight == false)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
                facingRight = true;
            }
            moveX = +1f;
        }
        
        Vector3 moveDir = new Vector3(moveX, moveY).normalized;

        transform.position += moveDir * SPEED * Time.deltaTime;
    }
}
