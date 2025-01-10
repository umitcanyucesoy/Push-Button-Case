using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Create", menuName = "Data/HandData", order = 0)]
    public class HandData : ScriptableObject
    {
        [Header("~~~~~~~~~ HAND DATA ~~~~~~~~~")]
        public float speed;
        public float forwardSpeed;
        public float minZ;
        public float maxZ;
    }
}