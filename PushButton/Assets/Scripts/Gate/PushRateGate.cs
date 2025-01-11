using System.Globalization;
using Hand;
using SFX;
using TMPro;
using UnityEngine;

namespace Gate
{
    public class PushRateGate : MonoBehaviour
    {
        [Header("~~~~~~~~~~~~ Gate Settings ~~~~~~~~~~~~")]
        [SerializeField] private TextMeshPro textMesh;
        public bool isGreenGate;
        private bool _hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered) return;
            if (!other.CompareTag("Hand")) return;

            _hasTriggered = true;

            float valueChange = GetNormalizedValueFromText();

            if (isGreenGate)
                HandManager.Instance.UpdatePressDuration(-valueChange); 
            else
                HandManager.Instance.UpdatePressDuration(valueChange);
            
            SfxManager.Instance.PlayGateSfx();
        }

        private float GetNormalizedValueFromText()
        {
            string text = textMesh.text.Trim(); 
            text = text.Replace("+", "").Trim();

            if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
            {
                float normalizedValue = value / 10f;
                return normalizedValue;
            }
            return 0;
        }
    }
}