﻿using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Inventory
{
    public class InventoryController2 : MonoBehaviour
    {
        public GameObject inventoryMenuPrefab;
        public List<CharacterItem> inventoryItems;
        
        
        public void Start()
        {
            inventoryItems = new List<CharacterItem>();
            
            var sword = gameObject.AddComponent<CharacterItem>();
            sword.CreateCharacterItem(0, "Sword", 1f, 0, 0, 0, 0);

            AddItem(sword);
        }

        public void AddItem(CharacterItem item)
        {
            inventoryItems.Add(item);
        }

        public void RemoveItem(CharacterItem item)
        {
            inventoryItems.Remove(item);
        }
    }
}