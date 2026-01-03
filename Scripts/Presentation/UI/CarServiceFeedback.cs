using System.Collections;
using TMPro;
using UnityEngine;

public sealed class CarServiceFeedback : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField, Min(0.2f)] private float duration = 1.5f;

    private Coroutine _routine;

    private void Awake()
    {
        if (text != null)
            text.gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        if (text == null)
        {
            Debug.Log($"[CarServiceFeedback] {message}");
            return;
        }

        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        text.text = message;
        text.gameObject.SetActive(true);

        // Попап ставит timeScale = 0 → только Realtime
        yield return new WaitForSecondsRealtime(duration);

        text.gameObject.SetActive(false);
        _routine = null;
    }
}