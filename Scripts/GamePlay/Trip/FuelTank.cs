using System;

public sealed class FuelTank
{
    public float MaxFuel { get; }
    public float CurrentFuel { get; private set; }

    public bool IsEmpty => CurrentFuel <= 0f;

    public event Action<float, float> OnFuelChanged;
    public event Action OnEmpty;

    public FuelTank(float maxFuel)
    {
        MaxFuel = maxFuel;
        CurrentFuel = maxFuel;
    }

    public void Consume(float amount)
    {
        if (IsEmpty || amount <= 0f)
            return;

        CurrentFuel -= amount;

        if (CurrentFuel <= 0f)
        {
            CurrentFuel = 0f;
            OnFuelChanged?.Invoke(CurrentFuel, MaxFuel);
            OnEmpty?.Invoke();
            return;
        }

        OnFuelChanged?.Invoke(CurrentFuel, MaxFuel);
    }

    public void Refill(float amount)
    {
        if (amount <= 0f)
            return;

        CurrentFuel = Math.Min(CurrentFuel + amount, MaxFuel);
        OnFuelChanged?.Invoke(CurrentFuel, MaxFuel);
    }

    public void SetFull()
    {
        CurrentFuel = MaxFuel;
        OnFuelChanged?.Invoke(CurrentFuel, MaxFuel);
    }
}