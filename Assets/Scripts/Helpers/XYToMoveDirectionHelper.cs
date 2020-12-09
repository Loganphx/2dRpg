using UnityEngine;

namespace DefaultNamespace.Helpers
{
    public class XYToMoveDirectionHelper
    {
        public XYToMoveDirectionHelper()
        {
        }

        public static MoveDirection XYToMoveDirection(Vector2Int vector2Int, Vector2Int targetLocation)
        {
            if (targetLocation.x - vector2Int.x > 0)
                return MoveDirection.Right;
            if (targetLocation.x - vector2Int.x < 0)
                return MoveDirection.Left;
            if (targetLocation.y - vector2Int.y > 0)
                return MoveDirection.Up;
            if (targetLocation.y - vector2Int.y < 0)
                return MoveDirection.Down;

            return MoveDirection.None;
        }
    }
}