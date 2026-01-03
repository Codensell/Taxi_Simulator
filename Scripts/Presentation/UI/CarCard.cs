using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CarCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image carImage;
    [SerializeField] private Button priceButton;
    [SerializeField] private TMP_Text priceText;

    [Header("Car")]
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private int price;

    [Header("Links")]
    [SerializeField] private TaxiCarSwitcher carSwitcher;
    [SerializeField] private CarServicePopup popupToClose;

    [Header("Refs")]
    [SerializeField] private GameSession gameSession;
    private CarServiceFeedback _feedback;

    private void Awake()
    {
        _feedback = GetComponentInParent<CarServiceFeedback>(true);

        if (priceText != null)
            priceText.text = $"{price}";

        if (priceButton != null)
            priceButton.onClick.AddListener(OnPriceClicked);
    }

    private void OnPriceClicked()
    {
        if (carSwitcher == null || carPrefab == null || gameSession == null)
            return;

        MoneyWallet wallet = gameSession.Money;
        if (wallet == null)
            return;

        if (!wallet.TrySpend(price))
        {
            _feedback?.Show("Недостаточно денег!");
            return;
        }

        carSwitcher.SwitchTo(carPrefab);
        popupToClose?.Close();
    }

    private void OnDestroy()
        {
            if (priceButton != null)
                priceButton.onClick.RemoveListener(OnPriceClicked);
        }
}