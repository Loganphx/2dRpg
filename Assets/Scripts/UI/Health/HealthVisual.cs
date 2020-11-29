using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Health
{
    public class HealthVisual : MonoBehaviour
    {
        public List<UIHeart> uiHearts = new List<UIHeart>();  
        private Sprite heartSprite;
        public GameObject heartPrefab;
        public Transform heartPanel;
        public int numberOfHearts = 3;
        
        private void Awake()
        {
            for (var i = 0; i < numberOfHearts; i++)
            {
                var instance = Instantiate(heartPrefab, heartPanel.position, Quaternion.identity);
                instance.transform.SetParent(heartPanel);
                instance.transform.localScale = new Vector3(1,1,1);
                uiHearts.Add(instance.GetComponentInChildren<UIHeart>());
            }
        }
        /// <summary>
        /// Updates slot with heart
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="heart"></param>
        public void UpdateSlot(int slot, Heart heart)
        {

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
        }
    }
    
}