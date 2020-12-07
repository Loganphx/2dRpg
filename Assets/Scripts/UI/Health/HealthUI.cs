using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UI.Health
{
    public class HealthUI : MonoBehaviour
    {
        public List<UIHeart> uiHearts = new List<UIHeart>();  
        private Sprite _heartSprite;
        public GameObject heartPrefab;
        public Transform heartSlots;
        public float numberOfHearts;
        public int lastDamagedHeart;
        public Sprite halfHeart;
        
        
        private void Start()
        {
            for (var i = 0; i < numberOfHearts; i++)    
            {
                var instance = Instantiate(heartPrefab, heartSlots.position, Quaternion.identity);
                instance.transform.SetParent(heartSlots);
                instance.transform.localScale = new Vector3(1,1,1);
                instance.tag = "Heart";
                uiHearts.Add(instance.GetComponentInChildren<UIHeart>());
            }
            lastDamagedHeart = uiHearts.Count - 1;
            numberOfHearts = uiHearts.Count;
            

        }
        /// <summary>
        /// Updates slot with heart
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="heart"></param>
        private void UpdateSlot(int slot, Heart heart)
        {
            if (heart.amount == 0)
            {
                numberOfHearts--;
            }
            uiHearts[slot].UpdateHeart(heart);
        }

        /// <summary>
        /// Adds item to next empty slot
        /// </summary>
        /// <param name="heart"></param>
        public void AddNewItem(Heart heart)
        {
            if(heart != null)
            {
                for(var i = 0; i < uiHearts.Count; i++)
                {
                    var uiHeart = uiHearts[i];
                    if(uiHeart.heart == null)
                    {
                        uiHeart.heart = heart;
                        uiHeart.UpdateHeart(heart);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes item from inventory
        /// </summary>
        /// <param name="heart"></param>
        public void RemoveItem(Heart heart)
        {
            UpdateSlot(uiHearts.FindIndex(i => i.heart == heart), null);
        }


        /// <summary>
        /// Removes heart from specified slot
        /// </summary>
        /// <param name="heartSlot"></param>
        public void RemoveItem(int heartSlot)
        {
            UpdateSlot(heartSlot, null);
            numberOfHearts--;
            if (numberOfHearts == 0)
            {
                Debug.Log("You have died.");
            }
        }

        public void AddHearts(int amount)
        {
            Debug.Log(amount);
            var index = 0;
            for (var i = uiHearts.Count-1; i > 0; i--)
            {
                if (uiHearts[i].heart.amount == 1)
                {
                    index = i;
                    break;
                }
            }

            for (var i = 0; i < amount; i++)    
            {
                var instance = Instantiate(heartPrefab, heartSlots.position, Quaternion.identity);
                instance.transform.SetParent(heartSlots);
                instance.transform.localScale = new Vector3(1,1,1);
                instance.tag = "Heart";
                var temp = uiHearts[index];
                uiHearts.Insert(index-1,instance.GetComponentInChildren<UIHeart>());
                lastDamagedHeart = uiHearts.Count;
                numberOfHearts = uiHearts.Count;
                index++;
            }
            var hearts = GameObject.FindGameObjectsWithTag ("Heart");
            var oldData = new List<UIHeart>();
            foreach (var heart in hearts)
            {
                oldData.Add(heart.GetComponentInChildren<UIHeart>());
                Destroy(heart);
            }
            Debug.Log(oldData.Count);
            Debug.Log(uiHearts.Count);
            var tempData = new List<UIHeart>();
            foreach (var heart in oldData)
            {
                if (heart.heart.amount == 1)
                {
                    tempData.Add(heart);
                }
            }

            foreach (var heart in oldData)
            {
                if (heart.heart.amount != 1)
                {
                    tempData.Add(heart);
                }
            }
            for(var i = 0; i < uiHearts.Count; i++)
            {
                var instance = Instantiate(heartPrefab, heartSlots.position, Quaternion.identity);
                instance.transform.SetParent(heartSlots);
                instance.transform.localScale = new Vector3(1,1,1);
                instance.tag = "Heart";
                uiHearts[i] = instance.GetComponentInChildren<UIHeart>();
                uiHearts[i].UpdateHeart(tempData[i].heart);
                lastDamagedHeart = uiHearts.Count;
                numberOfHearts = uiHearts.Count;
            }
            
        }
    }
    
}