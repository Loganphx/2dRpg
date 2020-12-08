using System.Collections.Generic;
using System.Security.Cryptography;
using Items;
using UI;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public List<UIItem> uIItems = new List<UIItem>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public int numberOfSlots = 10;

    private void Awake()
    {
        for (var i = 0; i < numberOfSlots; i++)
        {
            var instance = Instantiate(slotPrefab, slotPanel.position, Quaternion.identity);
                instance.transform.SetParent(slotPanel);
                instance.transform.localScale = new Vector3(1,1,1);
                uIItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }
    /// <summary>
    /// Updates Inventory slot to passed item.
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="item"></param>
    public void UpdateSlot(int slot, Item item)
    {

        uIItems[slot].UpdateItem(item);
    }

    /// <summary>
    /// Adds item to next empty slot
    /// </summary>
    /// <param name="item"></param>
    public void AddNewItem(Item item)
    {
        if(item != null)
        {
            for(var i = 0; i < uIItems.Count; i++)
            {
                var uiItem = uIItems[i];
                if(uiItem.Item == null)
                {
                    uiItem.Item = item;
                    uiItem.UpdateItem(item);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Removes item from inventory
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        UpdateSlot(uIItems.FindIndex(i => i.Item == item), null);
    }

    
    /// <summary>
    /// Removes item from specified slot
    /// </summary>
    /// <param name="inventorySlot"></param>
    public void RemoveItem(int inventorySlot)
    {
        UpdateSlot(inventorySlot, null);
    }
}
