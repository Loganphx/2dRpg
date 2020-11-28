﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class FollowMousePosition : MonoBehaviour
    {
        public Camera mainCamera;
        // Update is called once per frame
        void Update()
        {
            transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 700));
            
        }
    }
}

