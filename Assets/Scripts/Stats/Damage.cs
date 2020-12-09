using UnityEngine;

namespace Stats
{
    public class Damage : MonoBehaviour
    {
        public float baseDamage;
        public float attackRate;
        public float attackRange;


        /// <summary>
        /// Damages Character and will trigger animation on death threshold.
        /// </summary>
        /// <param name="colliderObject"></param>
        /// <param name="projectileDamage"></param>
        public void DamageCharacter(Collider2D colliderObject, float projectileDamage)
        {
            colliderObject.GetComponent<Health>().ChangeHealth(projectileDamage);
            // colliderHealth.;
            
            // return colliderHealth.baseHealth;
        }
    }
}