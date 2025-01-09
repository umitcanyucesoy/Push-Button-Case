using System.Globalization;
using Hand;
using UnityEngine;
using TMPro;

public class GateTrigger : MonoBehaviour
{
    [Header("Gate Settings")]
    public bool isGreenGate; // Yeşil gate mi, kırmızı gate mi?
    [SerializeField] private TextMeshPro textMesh; // Gate üzerindeki yazı
    private bool _hasTriggered = false; // Trigger kontrolü

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return; // Daha önce tetiklendiyse işlemi durdur
        if (!other.CompareTag("Hand")) return; // Sadece "Hand" tag'i olan objeler tetiklesin

        _hasTriggered = true; // İlk tetiklemeden sonra bu true olacak

        int valueChange = GetValueFromText();

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