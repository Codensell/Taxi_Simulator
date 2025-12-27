using UnityEngine;

public sealed class GameSession : MonoBehaviour
{
    public MoneyWallet Money { get; private set; }
    public TripTimer TripTimer { get; private set; }
    public FuelTank FuelTank { get; private set; }

    [SerializeField] private float maxFuelLiters = 20f;

    private void Awake()
    {
        Money = GetComponent<MoneyWallet>();
        if (Money == null)
            Debug.LogError("[GameSession] MoneyWallet component is missing on GameSession object");

        TripTimer = new TripTimer();
        FuelTank = new FuelTank(maxFuelLiters);
    }

    private void Start()
    {
        FuelTank.OnEmpty += OnFuelEmpty;
    }

    private void Update()
    {
        TripTimer.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (FuelTank != null)
            FuelTank.OnEmpty -= OnFuelEmpty;
    }

    private void OnFuelEmpty()
    {
        Debug.Log("[GameSession] Fuel is empty");
    }
}