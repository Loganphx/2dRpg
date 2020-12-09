using DefaultNamespace;
using UnityEngine;

namespace Helpers
{
    public class MoveDirectionToVelocity
    {
        public Vector2 Velocity = Vector2.zero;

        public MoveDirectionToVelocity(MoveDirection moveDirection)
        {
            switch (moveDirection)
            {
                case MoveDirection.Up:
                    Velocity = Vector2.up;
                    break;
                case MoveDirection.Down:
                    Velocity = Vector2.down;
                    break;
                case MoveDirection.Left:
                    Velocity = Vector2.left;
                    break;
                case MoveDirection.Right:
                    Velocity = Vector2.right;
                    break;
            }
        }
    }
}