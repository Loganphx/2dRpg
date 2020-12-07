﻿using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        public List<Item> characterItems = new List<Item>();
        private ItemDatabase _itemDatabase;
        public UIInventory inventoryUI;
        public void Start()
        {
            _itemDatabase = GameObject.Find("Item Database").GetComponent<ItemDatabase>();

            GiveItem("Sword");
            foreach (var item in _itemDatabase.items)
            {
                GiveItem(item.Value.itemName);
            }
        }
        
        
        /// <summary>
        /// Gets item from database and adds to inventory
        /// </summary>
        /// <param name="itemName"></param>
        public void GiveItem(string itemName)
        {
            var itemToAdd = _itemDatabase.GetItem(itemName);
            characterItems.Add(itemToAdd);
            inventoryUI.AddNewItem(itemToAdd);    

        }
        
        
        /// <summary>
        /// Checks If Item exists in Inventory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item CheckForItem(int id)
        {
            return characterItems.Find(item => item.id == id);
        }
        
        
        /// <summary>
        /// Deletes Item from Inventory
        /// </summary>
        /// <param name="id"></param>
        public void RemoveItem(int id)
        {
            var item = CheckForItem(id);
            if (item == null) return;
            characterItems.Remove(item);
            inventoryUI.RemoveItem(item);
        }
    }
}
