﻿using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

namespace Items
{

    public class ItemDatabase : MonoBehaviour
    {
        public Dictionary<int, Item> items = new Dictionary<int, Item>();

        private void Awake()
        {
            BuildDatabase();
        }
        
        /// <summary>
        /// Gets item from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item GetItem(int id)
        {
            return items[id];
        }
        
        
        /// <summary>
        /// Gets item from database
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Item GetItem(string name)
        {
            foreach(var item in items)
            {
                if (item.Value.itemName == name)
                {
                    return item.Value;
                }
            }

            return null;
        }
        
        
        /// <summary>
        /// Builds database
        /// </summary>
        void BuildDatabase()
        {
            var icon = Resources.Load<Sprite>("Sprites/sword");
            var appleIcon = Resources.Load<Sprite>("Sprites/apple");
            var potionIcon = Resources.Load<Sprite>("Sprites/hp");
            var sword = new Item(0, "Sword", "A sword kappa", icon, 1, 0, 5, 0, 0);
            var apple = new Item(1, "Apple", "Restores 15hp and gives 2 move speed temporarily", appleIcon, 0, 0, 0, 15, 2);
            var healthPotion = new Item(2, "Health Potion", "Restores 10 hp!", potionIcon, 0, 0, 0, 10 ,0);
            var axe = new Item(3, "Axe", "Axe?", Resources.Load<Sprite>("Sprites/axe"), 15, 0, 2, 0, 0);
            var meat = new Item(4, "Meat", "MEAT?", Resources.Load<Sprite>("Sprites/meat"), 0, 0, 2, 5, 0);
            var shield = new Item(5, "Shield", "Steel Shield", Resources.Load<Sprite>("Sprites/shield"), 0, 0, 2, 0, 0);
            var hp = new Item(6, "Potion", "Potion...", Resources.Load<Sprite>("Sprites/hp"), 0, 0, 2, 0, 0);


            items = new Dictionary<int, Item> {{sword.id, sword}, {apple.id, apple}, {healthPotion.id, healthPotion}, {axe.id, axe}, {meat.id, meat}, {shield.id, shield}, {hp.id, hp}};
        }
    }
}