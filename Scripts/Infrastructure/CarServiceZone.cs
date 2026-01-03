using TMPro;
using UnityEngine;

public sealed class CarServiceZone : MonoBehaviour
{
    [SerializeField] private CarServicePopup popup;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string promptMessage = "Жми F открыть автосервис!";

    private bool _playerInside;

    private void Awake()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_playerInside)
            return;

        if (Input.GetKeyDown(KeyCode.F))
            popup?.Toggle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _playerInside = true;
        ShowPrompt(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _playerInside = false;
        ShowPrompt(false);

        // чтобы окно не оставалось открытым при выезде
        if (popup != null && popup.IsOpen)
            popup.Close();
    }

    private void ShowPrompt(bool show)
    {
        if (promptText == null)
            return;

        promptText.text = promptMessage;
        promptText.gameObject.SetActive(show);
    }
}