﻿using System;
using System.Collections;
using DefaultNamespace;
using Ludiq;
using Player;
using UnityEngine;
using Projectiles;
using Stats;

namespace Projectiles.Fireball
{
    public class ProjectileController : Projectile 
    {
        private SpriteRenderer _spriteRenderer;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            StartCoroutine(DestroyCoroutine());
        }
        

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void OnTriggerEnter2D(Collider2D collidesWith)
        {
            base.OnTriggerEnter2D(collidesWith);
            
            if (collidesWith.CompareTag("Player") || collidesWith.gameObject.name == gameObject.name)
            {
                return;
            }
            
            Destroy(this.gameObject);
        }
        
        
        private IEnumerator DestroyCoroutine()
        {
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(6);
            // animation.SetTrigger("destroy");
            Destroy(gameObject);
        }
    }
}