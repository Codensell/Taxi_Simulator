using UnityEngine;

public class TaxiPickupInteractor : MonoBehaviour
{
    public Transform CurrentDestination { get; private set; }

    private PassengerPickup _currentPassenger;
    private PassengerPickup _lastPassenger;
    [SerializeField] private ArrowTarget arrow;
    [SerializeField] private DestinationArrow destinationArrow;
    [SerializeField] private float dropDistance = 2.5f;
    [SerializeField] private TaxiNavArrow navArrow;
    [SerializeField] private MoneyWallet moneyWallet;
    [SerializeField] private int minReward = 10;
    [SerializeField] private int maxReward = 20;



    private void Update()
{
    // ---------- ПОДБОР ПАССАЖИРА ----------
    if (_currentPassenger != null && Input.GetKeyDown(KeyCode.E))
    {
        Debug.Log("[TaxiPickupInteractor] E pressed");

        if (!_currentPassenger.CanBePickedUp())
        {
            Debug.LogWarning("[TaxiPickupInteractor] Passenger cannot be picked");
            return;
        }

        _lastPassenger = _currentPassenger;

        _currentPassenger.Pickup();
        Debug.Log("[TaxiPickupInteractor] Passenger picked up");

        CurrentDestination = _currentPassenger.Destination;

        if (CurrentDestination == null)
        {
            Debug.LogError("[TaxiPickupInteractor] Destination is NULL after pickup");
        }
        else
        {
            Debug.Log($"[TaxiPickupInteractor] Destination set: {CurrentDestination.name}");

            if (navArrow != null)
            {
                navArrow.Show(CurrentDestination);
                Debug.Log("[TaxiPickupInteractor] NavArrow shown");
            }
            else
            {
                Debug.LogError("[TaxiPickupInteractor] navArrow is NULL");
            }

            if (destinationArrow != null)
            {
                destinationArrow.Show(CurrentDestination, transform);
                Debug.Log("[TaxiPickupInteractor] DestinationArrow shown");
            }
            else
            {
                Debug.LogError("[TaxiPickupInteractor] destinationArrow is NULL");
            }
        }

        _currentPassenger = null;
    }

    // ---------- ПРОВЕРКА ДОЕЗДА ----------
    if (CurrentDestination == null || _lastPassenger == null)
        return;

    float distance = Vector3.Distance(transform.position, CurrentDestination.position);

    if (distance <= dropDistance)
    {
        Debug.Log("[TaxiPickupInteractor] Arrived at destination");
        int reward = Random.Range(minReward, maxReward + 1);
        moneyWallet.Add(reward);

        Debug.Log($"[TaxiPickupInteractor] Money earned: {reward}");

        _lastPassenger.ReturnToStart();

        if (navArrow != null)
        {
            navArrow.Hide();
            Debug.Log("[TaxiPickupInteractor] NavArrow hidden");
        }

        if (destinationArrow != null)
        {
            destinationArrow.Hide();
            Debug.Log("[TaxiPickupInteractor] DestinationArrow hidden");
        }

        _lastPassenger = null;
        CurrentDestination = null;
    }
}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[TaxiPickupInteractor] Trigger ENTER: {other.name}");

        PassengerPickup passenger =
            other.GetComponentInParent<PassengerPickup>();

        if (passenger == null)
        {
            Debug.Log("[TaxiPickupInteractor] No PassengerPickup found in parent");
            return;
        }

        Debug.Log("[TaxiPickupInteractor] Passenger assigned");
        _currentPassenger = passenger;
    }


    private void OnTriggerExit(Collider other)
    {
        PassengerPickup passenger =
            other.GetComponentInParent<PassengerPickup>();

        if (passenger == null)
            return;

        if (_currentPassenger != passenger)
            return;

        _currentPassenger = null;
    }
}