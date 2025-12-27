using UnityEngine;

public class TaxiPickupInteractor : MonoBehaviour
{
    public Transform CurrentDestination { get; private set; }

    private PassengerPickup _currentPassenger;
    private PassengerPickup _lastPassenger;

    [SerializeField] private ArrowTarget arrow;
    [SerializeField] private DestinationArrow destinationArrow;
    [SerializeField] private TaxiNavArrow navArrow;
    [SerializeField] private GameSession gameSession;

    [SerializeField] private float dropDistance = 5f;
    [SerializeField] private int minReward = 10;
    [SerializeField] private int maxReward = 20;

    private void Start()
    {
        gameSession.TripTimer.OnExpired += OnTripExpired;
    }

    private void OnDestroy()
    {
        if (gameSession?.TripTimer == null)
            return;

        gameSession.TripTimer.OnExpired -= OnTripExpired;
    }

    private void Update()
    {
        // ---------- ПОДБОР ПАССАЖИРА ----------
        if (_currentPassenger != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!_currentPassenger.CanBePickedUp())
                return;

            _lastPassenger = _currentPassenger;
            _currentPassenger = null;

            _lastPassenger.Pickup();

            gameSession.TripTimer.Start(30f);

            CurrentDestination = _lastPassenger.Destination;

            if (CurrentDestination != null)
            {
                navArrow?.Show(CurrentDestination);
                destinationArrow?.Show(CurrentDestination, transform);
            }
        }

        // ---------- ПРОВЕРКА ДОЕЗДА ----------
        if (_lastPassenger == null || CurrentDestination == null)
            return;

        float distance = Vector3.Distance(transform.position, CurrentDestination.position);

        if (distance <= dropDistance)
        {
            gameSession.TripTimer.Stop();
            FinishTrip(success: true);
        }
    }

    private void OnTripExpired()
    {
        FinishTrip(success: false);
    }

    private void FinishTrip(bool success)
    {
        if (_lastPassenger == null)
            return;

        if (success)
        {
            int reward = Random.Range(minReward, maxReward + 1);
            gameSession.Money.Add(reward);
            Debug.Log($"[TaxiPickupInteractor] Trip success. Earned {reward}");
        }
        else
        {
            Debug.Log("[TaxiPickupInteractor] Trip failed. No reward");
        }

        _lastPassenger.ReturnToStart();

        navArrow?.Hide();
        destinationArrow?.Hide();

        _lastPassenger = null;
        CurrentDestination = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        PassengerPickup passenger = other.GetComponentInParent<PassengerPickup>();
        if (passenger != null)
            _currentPassenger = passenger;
    }

    private void OnTriggerExit(Collider other)
    {
        PassengerPickup passenger = other.GetComponentInParent<PassengerPickup>();
        if (passenger != null && _currentPassenger == passenger)
            _currentPassenger = null;
    }
}
