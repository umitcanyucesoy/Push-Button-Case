using System;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("~~~~~~ Follow Settings ~~~~~~")] 
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        
        private void LateUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}