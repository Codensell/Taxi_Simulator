using UnityEngine;

public sealed class CarServicePopup : MonoBehaviour
{
    public bool IsOpen => _isOpen;

    [SerializeField] private GameObject panelRoot;
    [SerializeField] private bool pauseGameWhileOpen = true;

    private bool _isOpen;

    private void Awake()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    public void Toggle()
    {
        if (_isOpen) Close();
        else Open();
    }

    public void Open()
    {
        if (_isOpen || panelRoot == null)
            return;

        _isOpen = true;
        panelRoot.SetActive(true);

        if (pauseGameWhileOpen)
            SetPaused(true);
    }

    public void Close()
    {
        if (!_isOpen || panelRoot == null)
            return;

        _isOpen = false;
        panelRoot.SetActive(false);

        if (pauseGameWhileOpen)
            SetPaused(false);
    }

    private static void SetPaused(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
        AudioListener.pause = paused;
    }
}