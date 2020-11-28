using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Items;
using UnityEngine;

public class PlaceItemIntoInventory : MonoBehaviour
{
    public Item item;
    public InventoryController inventory;
    
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("PickupItem"))
        {
            inventory.GiveItem(other.gameObject.GetComponent<CharacterItem>().Name);
            Destroy(other.gameObject);
        }
     
}