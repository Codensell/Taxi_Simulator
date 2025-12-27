using System.Collections;
using TMPro;
using UnityEngine;

public sealed class TripResultUI : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private float showDuration = 2f;
    [SerializeField] private GameObject visualsRoot;

    private Coroutine _hideRoutine;

    private void Awake()
    {
        visualsRoot.SetActive(false);
    }

    private void Start()
    {
        session.TripTimer.OnStopped += OnTripSuccess;
        session.TripTimer.OnExpired += OnTripFailed;
    }

    private void OnDestroy()
    {
        if (session?.TripTimer == null)
            return;

        session.TripTimer.OnStopped -= OnTripSuccess;
        session.TripTimer.OnExpired -= OnTripFailed;
    }

    private void OnTripSuccess()
    {
        Show("УСПЕХ!", Color.green);
    }

    private void OnTripFailed()
    {
        Show("ПОТРАЧЕНО!", Color.red);
    }

    private void Show(string text, Color color)
    {
        if (_hideRoutine != null)
            StopCoroutine(_hideRoutine);

        resultText.text = text;
        resultText.color = color;

        visualsRoot.SetActive(true);
        _hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showDuration);
        visualsRoot.SetActive(false);
    }

}