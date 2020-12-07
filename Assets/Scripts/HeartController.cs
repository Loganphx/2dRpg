using System;
using System.Collections.Generic;
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
        var index = int.Parse(heartUI.lastDamagedHeart + "");
        if (index >= heartUI.uiHearts.Count) return;
        var heart = heartUI.uiHearts[index];
        Debug.Log("ERROR?");
        if (heart.heart.amount == 0)
        {
            if (damage == -1)
            {
                heart.heart.amount += 1;
                heart.UpdateHeart(heart.heart);
                Debug.Log(heart.heart.amount);
                gameObject.GetComponent<CharacterStats>().HealthController.ChangeHealth(damage);
                if (Math.Abs(heart.heart.amount - 1) < 0.05)
                {
                    heartUI.lastDamagedHeart += 1;
                    heartUI.numberOfHearts++;
                }
            }

        }
        else if (heart.heart.amount == 0.5)
        {
            if (damage == -1)
            {
                for (var i = 0; i < 2; i++)
                {
                    if (heartUI.lastDamagedHeart <= heartUI.uiHearts.Count - 1)
                    {
                        heart = heartUI.uiHearts[heartUI.lastDamagedHeart];
                        heart.heart.amount += 0.5f;
                        heart.UpdateHeart(heart.heart);
                        gameObject.GetComponent<CharacterStats>().HealthController.ChangeHealth(damage);
                        if (Math.Abs(heart.heart.amount - 1) < 0.05)
                        {
                            if (heartUI.numberOfHearts == heartUI.uiHearts.Count) break;
                            heartUI.lastDamagedHeart++;
                            heartUI.numberOfHearts++;
                        }
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