using Ludiq;
using UnityEngine;

namespace Stats
{
    public class Health : MonoBehaviour
    {
        public float baseHealth;
        public float deathThreshold = 0;
        
        public void ChangeHealth(float amount)
        {
            baseHealth += amount;
            if (baseHealth <= 0)
            {
                // animate death 
                Destroy(this.GameObject());
            }
        }
    }
}