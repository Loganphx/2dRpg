using System.Collections.Generic;
 using Tiles;
 using UnityEngine;

namespace DefaultNamespace.AI.PathFinding
{
    public class MoveCost
    {
        public float OverAllDistanceAToB;
        public float Up;
        public float UpH;
        public float UpRight;
        public float UpRightH;
        public float UpLeft;
        public float UpLeftH;

        public float Down;
        public float DownH;
        public float DownRight;
        public float DownRightH;
        public float DownLeft;
        public float DownLeftH;

        public float Right;
        public float RightH;
        public float Left;
        public float LeftH;

        public MoveCost(
            Vector3 startingVector3,
            Vector3 endingVector3,
            Dictionary<Vector3Int, WorldTile> obstaclePlaces,
            Dictionary<Vector3Int, WorldTile> availablePlaces,
            Dictionary<Vector3Int, WorldTile> visitedPlaces
        )
        {
            var sV = startingVector3;
            OverAllDistanceAToB = Vector3.Distance(startingVector3, endingVector3);
        
            var upKey = new Vector3Int((int) sV.x, (int) (sV.y + 1),0);
            var upRightKey = new Vector3Int((int) (sV.x + 1), (int) (sV.y + 1), 0);
            var upLeftKey = new Vector3Int((int) (sV.x - 1), (int) (sV.y + 1), 0);
            
            var downKey = new Vector3Int((int) sV.x, (int) (sV.y - 1), 0);
            var downRightKey = new Vector3Int((int) (sV.x + 1), (int) (sV.y - 1), 0);
            var downLeftKey = new Vector3Int((int) (sV.x - 1), (int) (sV.y - 1), 0);
            
            var rightKey =new Vector3Int((int) (sV.x + 1), (int) sV.y, 0);
            var leftKey = new Vector3Int((int) (sV.x - 1), (int) sV.y, 0);

            Up = 10;
            UpRight = 14;
            UpLeft = 14;

            Down = 10;
            DownRight = 14;
            DownLeft = 14;

            Right = 10;
            Left = 10;

            if (obstaclePlaces.ContainsKey(upKey) || visitedPlaces.ContainsKey(upKey) || availablePlaces.ContainsKey(upKey) == false) {Up = -1;UpH = -1;}
            else
            {
                UpH = Vector3.Distance(new Vector3(sV.x, sV.y + 1, 0), endingVector3);
            }
            
            if (obstaclePlaces.ContainsKey(upRightKey) || visitedPlaces.ContainsKey(upRightKey) || availablePlaces.ContainsKey(upRightKey) == false) {UpRight = -1; UpRightH = -1;}
            else {
                UpRightH = Vector3.Distance(new Vector3(sV.x + 1, sV.y + 1, 0), endingVector3);
            }
            
            if (obstaclePlaces.ContainsKey(upLeftKey) || visitedPlaces.ContainsKey(upLeftKey) || availablePlaces.ContainsKey(upLeftKey) == false)
            {
                UpLeft = -1;
                UpLeftH = -1;
            }
            else {
                UpLeftH = Vector3.Distance(new Vector3(sV.x - 1, sV.y + 1, 0), endingVector3);
            }
            
            if (obstaclePlaces.ContainsKey(downKey) || visitedPlaces.ContainsKey(downKey) || availablePlaces.ContainsKey(downKey) == false)
            {
                Down = -1;
                DownH = -1;
            }
            else {
                DownH = Vector3.Distance(new Vector3(sV.x, sV.y - 1, 0), endingVector3);
            }
            if (obstaclePlaces.ContainsKey(downRightKey) || visitedPlaces.ContainsKey(downRightKey) || availablePlaces.ContainsKey(downRightKey) == false)
            {
                DownRight = -1;
                DownRightH = -1;
            }
            else {
                DownRightH = Vector3.Distance(new Vector3(sV.x + 1, sV.y - 1, 0), endingVector3);
            }
            if (obstaclePlaces.ContainsKey(downLeftKey) || visitedPlaces.ContainsKey(downLeftKey) || availablePlaces.ContainsKey(downLeftKey) == false)
            {
                DownLeft = -1;
                DownLeftH = -1;
            }
            else {
                DownLeftH = Vector3.Distance(new Vector3(sV.x - 1, sV.y - 1, 0), endingVector3);
            }
            
            if (obstaclePlaces.ContainsKey(rightKey) || visitedPlaces.ContainsKey(rightKey)|| availablePlaces.ContainsKey(rightKey) == false)
            {
                Right = -1;
                RightH = -1;
            }
            else {
                RightH = Vector3.Distance(new Vector3(sV.x + 1, sV.y, 0), endingVector3);
            }
            if (obstaclePlaces.ContainsKey(leftKey) || visitedPlaces.ContainsKey(leftKey)|| availablePlaces.ContainsKey(leftKey) == false)
            {
                Left = -1;
                LeftH = -1;
            }
            else {
                LeftH = Vector3.Distance(new Vector3(sV.x - 1, sV.y - 1, 0), endingVector3);
            }
        }
    }
}