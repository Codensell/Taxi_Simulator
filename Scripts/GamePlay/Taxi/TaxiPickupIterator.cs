using System.Collections;
using TMPro;
using UnityEngine;

public class TaxiPickupInteractor : MonoBehaviour
{
    public event System.Action OnPassengerPickedUp;

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
    [SerializeField] private TMP_Text alreadyHasPassengerText;
    [SerializeField, Min(0.2f)] private float alreadyHasPassengerMessageDuration = 1.5f;

    private Coroutine _alreadyHasPassengerRoutine;
    [Header("Audio")]
	[SerializeField] private MusicDucker musicDucker;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip tripFailedClip;
    [SerializeField] private AudioClip tripSuccessClip;
    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

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
            if (_lastPassenger != null)
            {
                ShowAlreadyHasPassenger();
                return;
            }
            
            if (!_currentPassenger.CanBePickedUp())
                return;

            _lastPassenger = _currentPassenger;
            _currentPassenger = null;

            _lastPassenger.Pickup();

            gameSession.TripTimer.Start(60f);

            CurrentDestination = _lastPassenger.Destination;

            if (CurrentDestination != null)
            {
                navArrow?.Show(CurrentDestination);
                destinationArrow?.Show(CurrentDestination, transform);
            }
            OnPassengerPickedUp?.Invoke();
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
        PlaySfxWithDuck(tripSuccessClip);

        int reward = UnityEngine.Random.Range(minReward, maxReward + 1);
        gameSession.Money.Add(reward);
        Debug.Log($"[TaxiPickupInteractor] Trip success. Earned {reward}");
    }
    else
    {
        PlaySfxWithDuck(tripFailedClip);
        Debug.Log("[TaxiPickupInteractor] Trip failed. No reward");
    }

    _lastPassenger.ReturnToStart();

    navArrow?.Hide();
    destinationArrow?.Hide();

    _lastPassenger = null;
    CurrentDestination = null;
	}

	private void PlaySfxWithDuck(AudioClip clip, float fallbackSeconds = 1.5f)
	{
    if (clip == null)
        return;

    float seconds = clip.length;
    if (seconds <= 0.05f)
        seconds = fallbackSeconds;

    if (musicDucker != null)
        musicDucker.DuckForSeconds(seconds);

    if (audioSource != null)
        audioSource.PlayOneShot(clip);
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
    private void ShowAlreadyHasPassenger()
    {
        const string message = "Пассажир уже есть!";

        if (alreadyHasPassengerText == null)
        {
            Debug.Log($"[TaxiPickupInteractor] {message}");
            return;
        }

        if (_alreadyHasPassengerRoutine != null)
            StopCoroutine(_alreadyHasPassengerRoutine);

        _alreadyHasPassengerRoutine = StartCoroutine(ShowMessageRoutine(message, alreadyHasPassengerMessageDuration));
    }

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        alreadyHasPassengerText.text = message;
        alreadyHasPassengerText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        alreadyHasPassengerText.gameObject.SetActive(false);
        _alreadyHasPassengerRoutine = null;
    }

}
