using SFX;
using UnityEngine;

namespace Gate
{
    public class LengthGate : MonoBehaviour
    {
        private bool _hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered) return;
            if (!other.CompareTag("Hand")) return;

            _hasTriggered = true;

            Hand.HandManager.Instance.AddRowToHands();
            
            SfxManager.Instance.PlayGateSfx();
        }
        
    }
}