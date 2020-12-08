using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using Player;
using Stats;
using UI.Health;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    public List<Heart> characterHearts = new List<Heart>();
    public HealthUI heartUI;
    public Health healthComponent;

    public void Awake()
    {
        healthComponent = gameObject.GetComponent<Health>();
        heartUI.numberOfHearts = healthComponent.baseHealth;
    }

    /// <summary>
    /// Deals Damage to player and updates UI accordingly.
    /// </summary>
    /// <param name="damage"></param>
    public void DamagePlayer(float damage)
    {
        //Changes player HP.
        healthComponent.ChangeHealth(damage);

        //Runs logic to update player heart UI.
        var index = int.Parse(heartUI.numberOfHearts - 1 + "");
        var heart = heartUI.uiHearts[index];
        if (heart.heart.amount <= 0)
        {
            heart = heartUI.uiHearts[index - 1];
        }
        heartUI.lastDamagedHeart = index;
        heart.heart.amount -= damage;
        Debug.Log(heart.heart.amount);
        heart.UpdateHeart(heart.heart);
        if (heart.heart.amount <= 0)
        {
            heartUI.numberOfHearts--;
        }

    }

    public void HealPlayer(float damage)
    {
        if (-damage % 0.5 == 0)
        {
            var index = -1;
            var count = -damage / 0.5;
            var indexes = new List<int>();

            foreach (var item in heartUI.uiHearts)
                {
                    index++;
                    if (item.heart.amount < 1)
                    {
                        indexes.Add(index);
                    }
                }
            indexes.Sort();
            for (var i = 0; i < count; i++)
                {
                    if(indexes.Count >= 1)
                    {
                        var heart = heartUI.uiHearts[indexes[0]];         

                        Debug.Log(indexes[0]);
                        heart.heart.amount += 0.5f;
                        Debug.Log(heart.heart.amount);
                        heart.UpdateHeart(heart.heart);
                        gameObject.GetComponent<CharacterStats>().HealthController.ChangeHealth(-0.5f);
                        if (heart.heart.amount >= 1)
                        {
                            var ind = indexes[0];
                            if(heartUI.numberOfHearts < heartUI.uiHearts.Count)
                            {
                                heartUI.numberOfHearts++;
                                heartUI.lastDamagedHeart++; 

                                
                            }
                            indexes.RemoveAt(0);
                        }
                    }
                    
                }

        }

    }

    public void MaxHealth(int maxLife)
    {
        heartUI.AddHearts(maxLife);
        healthComponent.ChangeHealth(-maxLife);
    }
}