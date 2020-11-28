using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Item
    {
        public int id;
        public string itemName;
        public string description;
        public Sprite icon;

        #region Stats
        public float damage;
        public float addedProjectiles;
        public float attackSpeed;
        public float health;
        public float moveSpeed;
        #endregion
        
        /// <summary>
        /// Creates item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemName"></param>
        /// <param name="description"></param>
        /// <param name="icon"></param>
        /// <param name="damage"></param>
        /// <param name="addedProjectiles"></param>
        /// <param name="attackSpeed"></param>
        /// <param name="health"></param>
        /// <param name="moveSpeed"></param>
        public Item(int id, string itemName, string description, Sprite icon, float damage, float addedProjectiles, float attackSpeed, float health, float moveSpeed)
        {
            this.id = id;
            this.itemName = itemName;
            this.description = description;
            this.icon = icon;
            this.damage = damage;
            this.addedProjectiles = addedProjectiles;
            this.attackSpeed = attackSpeed;
            this.health = health;
            this.moveSpeed = moveSpeed;


        }
        
        /// <summary>
        /// Duplicates an item
        /// </summary>
        /// <param name="item"></param>
        public Item(Item item)
        {
            this.id = item.id;
            this.itemName = item.itemName;
            this.description = item.description;
            this.icon = item.icon;
            this.damage = item.damage;
            this.addedProjectiles = item.addedProjectiles;
            this.attackSpeed = item.attackSpeed;
            this.health = item.health;
            this.moveSpeed = item.moveSpeed;

        }
    }
}
