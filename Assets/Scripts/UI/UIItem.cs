﻿using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIItem : MonoBehaviour, IPointerClickHandler
    {
        public Item Item;
        private Image _spriteImage;
        private UIItem _selectedItem;
        
        // Start is called before the first frame update
        private void Awake()
        {
            _spriteImage = GetComponent<Image>();
            UpdateItem(Item);
            _selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();

        }
        
        /// <summary>
        /// Updates item's sprite and color
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(Item item)
        {
            this.Item = item;
            if (this.Item != null)
            {
                _spriteImage.color = new Color(255, 255, 255, 255);
                _spriteImage.sprite = this.Item.icon;
                return;
            }

            _spriteImage.color = Color.clear;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.Item != null)
            {
                if (_selectedItem.Item != null)
                {
                    var clone = new Item(_selectedItem.Item);
                    _selectedItem.UpdateItem(this.Item);
                    UpdateItem(clone);
                }
                else
                {
                    _selectedItem.UpdateItem(this.Item);
                    UpdateItem(null);
                }
            }
            else if (_selectedItem.Item != null)
            {
                UpdateItem(_selectedItem.Item);
                _selectedItem.UpdateItem(null);
            }
        }

        public void OnPointerDown()
        {
            
        }

        public void OnPointerUp()
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("DROP");
                }
        }
    }
}
