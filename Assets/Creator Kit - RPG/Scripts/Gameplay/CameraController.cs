using System;
using DefaultNamespace;
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

        public void MoveMiniMapCamera(MoveDirection moveDirection)
        {
            Debug.Log("MOVE CAMERA?");
            Debug.Log(transform.position);
            Debug.Log(moveDirection);
            var position = transform.position;

            if (moveDirection == MoveDirection.Down)
            {
                position = Vector3.Lerp(
                    position,
                    new Vector3(position.x, position.y - 1 * 10, position.z),
                    Time.deltaTime * 5
                );
            }

            if (moveDirection == MoveDirection.Left)
            {
                position = Vector3.Lerp(
                    position,
                    new Vector3(position.x - 1 * 10, position.y, position.z),
                    Time.deltaTime * 5
                );
            }

            if (moveDirection == MoveDirection.Right)
            {
                position = Vector3.Lerp(
                    position,
                    new Vector3(position.x + 1 * 10, position.y, position.z),
                    Time.deltaTime * 5
                );
            }

            if (moveDirection == MoveDirection.Up)
            {
                position = Vector3.Lerp(
                    position,
                    new Vector3(position.x, position.y + 1 * 10, position.z),
                    Time.deltaTime * 5
                );
            }
            transform.position = position;
            Debug.Log(transform.position);

        }
    }
}