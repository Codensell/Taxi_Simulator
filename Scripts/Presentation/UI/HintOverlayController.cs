using UnityEngine;
using Game.Infrastructure.Pause;

namespace Game.UI
{
    public sealed class HintOverlayController : MonoBehaviour
    {
        [SerializeField] private GameObject hintPanel;
        [SerializeField] private bool pauseGameWhileOpen = true;

        private bool _isOpen;

        private void Awake()
        {
            if (hintPanel == null)
                hintPanel = gameObject;

            hintPanel.SetActive(false);
        }

        public void Toggle()
        {
            if (_isOpen) Close();
            else Open();
        }

        public void Open()
        {
            _isOpen = true;
            hintPanel.SetActive(true);

            if (pauseGameWhileOpen)
                GamePause.SetPaused(this, true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Close()
        {
            _isOpen = false;
            hintPanel.SetActive(false);

            if (pauseGameWhileOpen)
                GamePause.SetPaused(this, false);
        }

        private void OnDisable()
        {
            if (pauseGameWhileOpen && _isOpen)
                GamePause.SetPaused(this, false);

            _isOpen = false;
        }
    }
}