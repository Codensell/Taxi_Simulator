using UnityEngine;
using System;

public sealed class MoneyWallet : MonoBehaviour
{
    public int Money { get; private set; }

    public int Amount => Money;

    public event Action<int> OnMoneyChanged;

    public void Add(int amount)
    {
        if (amount <= 0)
            return;

        Money += amount;
        OnMoneyChanged?.Invoke(Money);

        Debug.Log($"[MoneyWallet] Money added: {amount}, total = {Money}");
    }

    public bool Spend(int amount)
    {
        if (amount <= 0)
            return true;

        if (Money < amount)
            return false;

        Money -= amount;
        OnMoneyChanged?.Invoke(Money);

        Debug.Log($"[MoneyWallet] Money spent: {amount}, total = {Money}");
        return true;
    }
    public bool CanSpend(int amount)
    {
        if (amount <= 0)
            return true;

        return Money >= amount;
    }

    public bool TrySpend(int amount)
    {
        return Spend(amount);
    }

}

