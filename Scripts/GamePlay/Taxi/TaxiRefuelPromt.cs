using TMPro;
using UnityEngine;

public sealed class TaxiRefuelPrompt : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string fuelStationTag = "FuelStation";

    private int _contacts;

    private void Awake()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(fuelStationTag))
            return;

        _contacts++;
        UpdateView(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(fuelStationTag))
            return;

        _contacts = Mathf.Max(0, _contacts - 1);

        if (_contacts == 0)
            UpdateView(false);
    }

    private void UpdateView(bool visible)
    {
        if (promptText == null)
            return;

        if (visible)
            promptText.text = "Жми F заправиться!";

        promptText.gameObject.SetActive(visible);
    }
}