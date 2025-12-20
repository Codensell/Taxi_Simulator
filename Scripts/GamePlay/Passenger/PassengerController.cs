using UnityEngine;
public enum PassengerState
{
    Waiting,
    ReadyToPickup,
    PickedUp
}
public class PassengerController : MonoBehaviour
{
    [SerializeField] private GameObject passengerView;
    [SerializeField] private GameObject pickupTrigger;
    [SerializeField] private GameObject passengerMarker;

    public PassengerState State { get; private set; } = PassengerState.Waiting;

    private void Awake()
    {
        SetWaitingState();
    }

    public void EnterPickupZone()
    {
        Debug.Log($"[PassengerController] EnterPickupZone, state = {State}");

        if (State != PassengerState.Waiting)
            return;

        State = PassengerState.ReadyToPickup;
        passengerMarker.SetActive(true);
    }

    public void ExitPickupZone()
    {
        Debug.Log($"[PassengerController] ExitPickupZone, state = {State}");

        if (State != PassengerState.ReadyToPickup)
            return;

        SetWaitingState();
    }

    public void Pickup()
    {
        Debug.Log($"[PassengerController] Pickup called, state = {State}");

        if (State != PassengerState.ReadyToPickup)
            return;

        State = PassengerState.PickedUp;

        passengerView.SetActive(false);
        pickupTrigger.SetActive(false);
        passengerMarker.SetActive(false);

        Debug.Log("[PassengerController] Passenger PICKED UP");
    }

    private void SetWaitingState()
    {
        State = PassengerState.Waiting;
        passengerMarker.SetActive(false);
    }
}