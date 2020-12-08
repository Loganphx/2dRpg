using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Stats;
using UI.Health;
using UnityEditor.UIElements;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private float _damage;
    // Start is called before the first frame update
    void Start()
    {
        _damage = gameObject.GetComponent<Damage>().baseDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("HIT");
        if (!gameObject.CompareTag("Player"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var heartController = other.gameObject.GetComponent<HeartController>();
                if (_damage > 0)
                    heartController.DamagePlayer(_damage);
                else if (_damage < 0)
                    heartController.HealPlayer(_damage);
            }
            else
            {
            }
        }
    }
}
