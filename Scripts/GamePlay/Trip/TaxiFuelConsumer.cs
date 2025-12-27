using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class TaxiFuelConsumer : MonoBehaviour
{
    [SerializeField] private GameSession gameSession;

    [Header("Fuel")]
    [SerializeField] private float fuelPerSecond = 0.0667f;
    [SerializeField] private float moveThreshold = 0.1f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (gameSession == null || gameSession.FuelTank == null)
            return;

        if (gameSession.FuelTank.IsEmpty)
            return;

        if (_rigidbody.linearVelocity.sqrMagnitude < moveThreshold * moveThreshold)
            return;

        gameSession.FuelTank.Consume(fuelPerSecond * Time.deltaTime);
    }
}