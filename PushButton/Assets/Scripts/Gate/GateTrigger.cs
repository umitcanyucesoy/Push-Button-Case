using System.Globalization;
using Hand;
using UnityEngine;
using TMPro;

public class GateTrigger : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] private TextMeshPro textMesh;
    public bool isGreenGate;
    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;
        if (!other.CompareTag("Hand")) return;

        _hasTriggered = true;

        int valueChange = GetValueFromText();
        Debug.Log($"Gate Triggered! Value: {valueChange}, Is Green Gate: {isGreenGate}");

        if (isGreenGate)
        {
            HandManager.Instance.AddHands(valueChange);
        }
        else
        {
            HandManager.Instance.RemoveHands(valueChange);
        }
    }
    
    private int GetValueFromText()
    {
        string text = textMesh.text.Trim();
        Debug.Log("Gate Text Value: " + text);

        text = text.Replace("+", "").Trim();

        if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
        {
            Debug.Log("Parsed Value: " + value);
            return Mathf.RoundToInt(value);
        }

        Debug.LogError("Invalid format for gate value: " + text);
        return 0;
    }
}