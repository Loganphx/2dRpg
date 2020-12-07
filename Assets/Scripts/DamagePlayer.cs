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
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HeartController>().DamagePlayer(_damage);
        }
        else
        {
            other.gameObject.GetComponent<Health>().ChangeHealth(_damage);
        }

    
    }
}
