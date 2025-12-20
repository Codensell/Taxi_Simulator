using UnityEngine;
using System;

public sealed class MoneyWallet : MonoBehaviour
{
    public int Money { get; private set; }

    public event Action<int> OnMoneyChanged;


    public void Add(int amount)
    {
        if (amount <= 0)
            return;

        Money += amount;

        OnMoneyChanged?.Invoke(Money);

        Debug.Log($"[MoneyWallet] Money added: {amount}, total = {Money}");
    }

}

