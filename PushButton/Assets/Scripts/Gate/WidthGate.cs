using System.Globalization;
using Hand;
using SFX;
using UnityEngine;
using TMPro;

public class WidthGate : MonoBehaviour
{
    [Header("~~~~~~~~~~~~ Gate ELEMENTS ~~~~~~~~~~~~")]
    [SerializeField] private TextMeshPro textMesh;
    public bool isGreenGate;
    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;
        if (!other.CompareTag("Hand")) return;

        _hasTriggered = true;
        int valueChange = GetValueFromText();

        if (isGreenGate)
            HandManager.Instance.AddHands(valueChange);
        else
            HandManager.Instance.RemoveHands(valueChange);
        
        SfxManager.Instance.PlayGateSfx();
    }
    
    private int GetValueFromText()
    {
        string text = textMesh.text.Trim();
        text = text.Replace("+", "").Trim();

        if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
        {
            return Mathf.RoundToInt(value);
        }
        return 0;
    }
}