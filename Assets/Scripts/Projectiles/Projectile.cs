﻿﻿using System;
using System.Collections;
using DefaultNamespace;
 using Ludiq;
 using Stats;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public float projectileSpeed = 1;
        public float projectileDamage;
        public GameObject projectileGameObject;
        public GameObject projectileOwner;
        // Start is called before the first frame update
        public Rigidbody2D rigidBody;
        public MoveDirection moveDirection;
        public SpriteRenderer spriteRenderer;
        
        public float playerSpeed;

        // Start is called before the first frame update
        public virtual void Start()
        {
            var constant = 1f;
            //Start the coroutine we define below named ExampleCoroutine.
            if (moveDirection == MoveDirection.Up)
            {
                spriteRenderer.flipX = true;
                projectileGameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                rigidBody.velocity = Vector2.up + new Vector2(0, projectileSpeed);
                Debug.Log($"x: {rigidBody.velocity.x}, y: {rigidBody.velocity.y}");
            }            
            
            if (moveDirection == MoveDirection.UpRight)
            {
                spriteRenderer.flipX = false;
                projectileGameObject.transform.rotation = Quaternion.Euler(0, 0, 45);
                rigidBody.velocity = Vector2.up + Vector2.right + new Vector2(0, projectileSpeed); //+ playerSpeed);
                Debug.Log($"x: {rigidBody.velocity.x}, y: {rigidBody.velocity.y}");
            }            
            
            if (moveDirection == MoveDirection.UpLeft)
            {
                projectileGameObject.transform.rotation = Quaternion.Euler(0, 0, 120);
                rigidBody.velocity = Vector2.up + Vector2.left + new Vector2(0, projectileSpeed); // + playerSpeed);
                Debug.Log($"x: {rigidBody.velocity.x}, y: {rigidBody.velocity.y}");
            }

            if (moveDirection == MoveDirection.Down)
            {
                spriteRenderer.flipX = true;
                rigidBody.transform.rotation = Quaternion.Euler(0, 0, -270);
                rigidBody.velocity = Vector2.down + new Vector2(0, - projectileSpeed);
            }
            
            if (moveDirection == MoveDirection.DownRight)
            {
                spriteRenderer.flipX = true;
                rigidBody.transform.rotation = Quaternion.Euler(0, 0, 45);
                rigidBody.velocity =
                    Vector2.down + Vector2.right + new Vector2(0, -projectileSpeed); // - playerSpeed );
            }
            
            if (moveDirection == MoveDirection.DownLeft)
            {
                rigidBody.transform.rotation = Quaternion.Euler(0, 0, 120);
                rigidBody.velocity = Vector2.down + Vector2.left + new Vector2(0, -projectileSpeed); // - playerSpeed );
            }

            if (moveDirection == MoveDirection.Right)
            {
                spriteRenderer.flipX = false;
                rigidBody.velocity = (Vector2.right + new Vector2(+projectileSpeed, 0)); // + playerSpeed, 0));
            }
            if (moveDirection == MoveDirection.Left)
            {
                spriteRenderer.flipX = true;
                rigidBody.velocity = (Vector2.left + new Vector2(-projectileSpeed, 0)); // - playerSpeed , 0));
            }
        }
        
        /// <summary>
        /// Overrideable Trigger For All Projectiles
        /// </summary>
        /// <param name="collidesWith">GameObject Collided With</param>
        public virtual bool OnTriggerEnter2D(Collider2D collidesWith)
        {
            Debug.Log($"PROJECTILE hit! {collidesWith.tag}, {collidesWith.name}");
            var tag = collidesWith.tag;
            switch (tag)
            {
                case "Player":
                    if (projectileOwner.CompareTag("AI"))
                    {
                        collidesWith.GetComponent<Damage>().DamageCharacter(collidesWith, -projectileDamage);
                        return true;
                    }
                    break;

                case "Walls":
                    break;

                case "AI":
                    Debug.Log("AI HIT");
                    if (projectileOwner.CompareTag("Player"))
                    {
                        Debug.Log("AI DAMAGED!!!");
                        collidesWith.GetComponent<Damage>().DamageCharacter(collidesWith, -projectileDamage);
                        return true;
                    }
                    break;
                
                case "Untagged":
                    switch (collidesWith.name)
                    {
                        case "Water":
                            return false;
                        case "Foreground":
                            return true;
                    }

                    break;
            }

            return false;
        }
    }
}
