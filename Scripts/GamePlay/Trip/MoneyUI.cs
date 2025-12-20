using TMPro;
using UnityEngine;

public sealed class MoneyUI : MonoBehaviour
{
    [SerializeField] private MoneyWallet wallet;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        if (wallet == null)
        {
            Debug.LogError("[MoneyUI] Wallet is NULL");
            return;
        }

        if (text == null)
        {
            Debug.LogError("[MoneyUI] Text is NULL");
            return;
        }

        text.text = $"$ {wallet.Money}";
        Debug.Log($"[MoneyUI] Init value: {wallet.Money}");
    }

    private void OnEnable()
    {
        if (wallet != null)
            wallet.OnMoneyChanged += UpdateText;
    }

    private void OnDisable()
    {
        if (wallet != null)
            wallet.OnMoneyChanged -= UpdateText;
    }

    private void UpdateText(int value)
    {
        Debug.Log($"[MoneyUI] UpdateText called: {value}");
        text.text = $"$ {value}";
    }
}