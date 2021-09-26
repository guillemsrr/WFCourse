using System.Collections.Generic;
using UnityEngine;

namespace WFCourse.Utilities
{
    public enum Direction
    {
        Front = 0,
        Back = 1,
        Left = 2,
        Right =  3,
        Up = 4,
        Down = 5
    }
    public class Directions
    {
        public static Vector3Int[] DirectionVectors =
        {
            Vector3Int.down,
            Vector3Int.up,
            Vector3Int.left,
            Vector3Int.right
        };
    }
}