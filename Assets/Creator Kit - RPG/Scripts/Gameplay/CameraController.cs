using System;
using UnityEngine;

namespace Scripts.Gameplay
{
    /// <summary>
    /// A simple camera follower class. It saves the offset from the
    ///  focus position when started, and preserves that offset when following the focus.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public Transform focus;
        public float smoothTime = 2;
        public float offsetFloat;
        Vector3 _offset;

        private void Awake()
        {
            UpdateCamera();
        }

        public void UpdateCamera()
        {
            var position = transform.position;
            var position1 = focus.position;
            
            position = Vector3.Lerp(
                position,
                new Vector3(position1.x, position1.y, position.z),
                Time.deltaTime * smoothTime
            );

            transform.position = position;
        }
    }
}    