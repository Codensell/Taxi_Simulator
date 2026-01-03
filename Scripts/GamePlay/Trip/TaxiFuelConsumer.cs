using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public sealed class TaxiFuelConsumer : MonoBehaviour
{
    [SerializeField] private GameSession gameSession;

    [Header("Fuel")]
    [SerializeField] private float fuelPerSecond = 0.0667f;
    [SerializeField] private float moveThreshold = 0.1f;
    [Header("Warning")]
    [SerializeField] private TMP_Text warningText;
    [SerializeField, Min(0.2f)] private float warningDuration = 1.5f;
    [SerializeField, Range(0.05f, 0.95f)] private float halfFuelThreshold = 0.5f;
    private FuelTank _subscribedTank;

    private bool _halfFuelWarned;
    private Coroutine _warningRoutine;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        EnsureFuelSubscription();

        if (gameSession == null || gameSession.FuelTank == null)
            return;

        if (gameSession.FuelTank.IsEmpty)
            return;

        if (_rigidbody.linearVelocity.sqrMagnitude < moveThreshold * moveThreshold)
            return;

        gameSession.FuelTank.Consume(fuelPerSecond * Time.deltaTime);
    }
    private void OnEnable()
    {
        if (gameSession != null && gameSession.FuelTank != null)
            gameSession.FuelTank.OnFuelChanged += OnFuelChanged;
    }

    private void OnDisable()
    {
        if (_subscribedTank != null)
            _subscribedTank.OnFuelChanged -= OnFuelChanged;

        _subscribedTank = null;
    }

    private void OnFuelChanged(float current, float max)
    {
        if (max <= 0f)
            return;

        float ratio = current / max;

        if (ratio > halfFuelThreshold)
        {
            _halfFuelWarned = false;
            return;
        }

        if (_halfFuelWarned)
            return;

        _halfFuelWarned = true;
        ShowWarning("Пора заправляться! Ищи любую заправку");
    }
    private void ShowWarning(string message)
    {
        if (warningText == null)
        {
            Debug.Log($"[TaxiFuelConsumer] {message}");
            return;
        }

        if (_warningRoutine != null)
            StopCoroutine(_warningRoutine);

        _warningRoutine = StartCoroutine(ShowWarningRoutine(message));
    }

    private System.Collections.IEnumerator ShowWarningRoutine(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(warningDuration);

        warningText.gameObject.SetActive(false);
        _warningRoutine = null;
    }
    private void EnsureFuelSubscription()
    {
        FuelTank tank = gameSession != null ? gameSession.FuelTank : null;

        if (ReferenceEquals(_subscribedTank, tank))
            return;

        if (_subscribedTank != null)
            _subscribedTank.OnFuelChanged -= OnFuelChanged;

        _subscribedTank = tank;

        if (_subscribedTank != null)
            _subscribedTank.OnFuelChanged += OnFuelChanged;
    }

    
}