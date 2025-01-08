using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Create", menuName = "Data/HandData", order = 0)]
    public class HandData : ScriptableObject
    {
        [Header("~~~~~~~~~ HAND DATA ~~~~~~~~~")]
        public int speed;
        public float forwardSpeed;
        public float minX;
        public float maxX;
    }
}