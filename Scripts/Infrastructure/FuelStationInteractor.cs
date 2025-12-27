using UnityEngine;

public sealed class FuelStationInteractor : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameSession gameSession;
    [SerializeField] private FuelStationUI stationUI;

    [Header("Input")]
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    [Header("Taxi Detection")]
    [SerializeField] private LayerMask taxiLayer;

    private bool _taxiInside;
    private bool _popupOpenedByThisStation;

    private readonly float[] _liters = { 2f, 5f, 10f };
    private readonly int[] _prices = { 4, 10, 20 };

    private void OnEnable()
    {
        if (stationUI == null)
            return;

        stationUI.SetOptionsText(
            "2 литра — $4",
            "5 литров — $10",
            "10 литров — $20");

        stationUI.OnOptionPayClicked += OnPayClicked;
    }

    private void OnDisable()
    {
        if (stationUI == null)
            return;

        stationUI.OnOptionPayClicked -= OnPayClicked;
    }

    private void Update()
    {
        if (!_taxiInside)
            return;

        if (!Input.GetKeyDown(interactKey))
            return;

        TogglePopup();
    }

    private void TogglePopup()
    {
        if (stationUI == null)
            return;

        if (stationUI.gameObject.activeSelf)
        {
            stationUI.Hide();
            _popupOpenedByThisStation = false;
        }
        else
        {
            stationUI.Show();
            _popupOpenedByThisStation = true;
        }
    }

    private void OnPayClicked(int optionIndex)
    {
        // Критично: игнорируем клики, если попап открывал НЕ этот интеректор,
        // или если такси уже не в зоне этой заправки.
        if (!_popupOpenedByThisStation || !_taxiInside)
            return;

        if (optionIndex < 0 || optionIndex >= _liters.Length)
            return;

        if (gameSession == null || gameSession.FuelTank == null || gameSession.Money == null)
            return;

        float liters = _liters[optionIndex];
        int price = _prices[optionIndex];

        if (!gameSession.Money.TrySpend(price))
        {
            stationUI.ShowNotEnoughMoney();
            return;
        }

        gameSession.FuelTank.Refill(liters);
        stationUI.Hide();
        _popupOpenedByThisStation = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsTaxi(other))
            return;

        _taxiInside = true;
        Debug.Log("[FuelStation] Taxi entered fuel zone");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsTaxi(other))
            return;

        _taxiInside = false;
        _popupOpenedByThisStation = false;

        if (stationUI != null)
            stationUI.Hide();

        Debug.Log("[FuelStation] Taxi exited fuel zone");
    }

    private bool IsTaxi(Collider other)
    {
        return ((1 << other.gameObject.layer) & taxiLayer.value) != 0;
    }
}
