using System.Collections;
using TMPro;
using UnityEngine;

public sealed class TaxiFirstTripTutorial : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private TaxiPickupInteractor taxiPickupInteractor;
    [SerializeField] private TMP_Text tutorialText;

    [Header("Messages")]
    [TextArea]
    [SerializeField] private string messageBeforePickup =
        "Видишь человека с желтой стрелкой над головой?\nПодъезжай ближе и нажми - E";

    [TextArea]
    [SerializeField] private string messageAfterPickup =
        "Следуй по желтой стрелке и доставь пассажира к месту!";

    [Header("Timing")]
    [SerializeField, Min(0.2f)] private float afterPickupDuration = 3.0f;

    private bool _shownSecond;
    private Coroutine _routine;

    private void Awake()
    {
        if (tutorialText != null)
            tutorialText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (taxiPickupInteractor != null)
            taxiPickupInteractor.OnPassengerPickedUp += HandlePassengerPickedUp;
    }

    private void OnDisable()
    {
        if (taxiPickupInteractor != null)
            taxiPickupInteractor.OnPassengerPickedUp -= HandlePassengerPickedUp;
    }

    private void Start()
    {
        ShowFirstHint();
    }

    private void ShowFirstHint()
    {
        if (tutorialText == null)
            return;

        tutorialText.text = messageBeforePickup;
        tutorialText.gameObject.SetActive(true);
    }

    private void HandlePassengerPickedUp()
    {
        if (_shownSecond)
            return;

        _shownSecond = true;

        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(ShowSecondThenHide());
    }

    private IEnumerator ShowSecondThenHide()
    {
        if (tutorialText == null)
            yield break;

        tutorialText.text = messageAfterPickup;
        tutorialText.gameObject.SetActive(true);

        yield return new WaitForSeconds(afterPickupDuration);

        tutorialText.gameObject.SetActive(false);
        _routine = null;

        // Можно отписаться, чтобы вообще больше не слушать
        if (taxiPickupInteractor != null)
            taxiPickupInteractor.OnPassengerPickedUp -= HandlePassengerPickedUp;
    }
}
