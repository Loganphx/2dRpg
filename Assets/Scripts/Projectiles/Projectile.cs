﻿using System;
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
                rigidBody.velocity = Vector2.up + new Vector2(0, playerSpeed + projectileSpeed);
                Debug.Log($"x: {rigidBody.velocity.x}, y: {rigidBody.velocity.y}");
            }

            if (moveDirection == MoveDirection.Down)
            {
                spriteRenderer.flipX = true;
                rigidBody.transform.rotation = Quaternion.Euler(0, 0, -270);
                rigidBody.velocity = Vector2.down + new Vector2(0, - playerSpeed - projectileSpeed);
            }

            if (moveDirection == MoveDirection.Right)
            {
                spriteRenderer.flipX = false;
                rigidBody.velocity = (Vector2.right + new Vector2(playerSpeed + projectileSpeed, 0));
            }
            if (moveDirection == MoveDirection.Left)
            {
                spriteRenderer.flipX = true;
                rigidBody.velocity = (Vector2.left + new Vector2(- playerSpeed - projectileSpeed, 0));
            }
        }
        
        /// <summary>
        /// Overrideable Trigger For All Projectiles
        /// </summary>
        /// <param name="collidesWith">GameObject Collided With</param>
        public virtual void OnTriggerEnter2D(Collider2D collidesWith)
        {
            Debug.Log($"PROJECTILE hit! {collidesWith.tag}, {collidesWith.name}");
            var tag = collidesWith.tag;
            switch (tag)
            {
                case "Player":
                    if (projectileOwner.CompareTag("AI"))
                        collidesWith.GetComponent<Damage>().DamageCharacter(collidesWith, -projectileDamage);
                    break;

                case "Walls":
                    break;

                case "AI":
                    if (projectileOwner.CompareTag("Player"))
                        collidesWith.GetComponent<Damage>().DamageCharacter(collidesWith, -projectileDamage);
                    break;
            }
        }
    }
}
