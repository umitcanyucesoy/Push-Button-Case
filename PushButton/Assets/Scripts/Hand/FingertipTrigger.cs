using System;
using Cysharp.Threading.Tasks;
using KeyboardButton;
using UnityEngine;

namespace Hand
{
    public class FingertipTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Button"))
                other.GetComponent<ButtonPress>().AnimateButton().Forget();
        }
}
}