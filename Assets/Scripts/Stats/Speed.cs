using UnityEngine;

namespace Stats
{
    public class Speed : MonoBehaviour
    {
        [Range(0, 15f)]
        public float baseSpeed;
        
        [Range(0, 5)]
        public float acceleration;
    }
}