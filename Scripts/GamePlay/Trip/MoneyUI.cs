using TMPro;
using UnityEngine;

public sealed class MoneyUI : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        text.text = $"$ {session.Money.Money}";
    }

    private void OnEnable()
    {
        session.Money.OnMoneyChanged += UpdateText;
    }

    private void OnDisable()
    {
        session.Money.OnMoneyChanged -= UpdateText;
    }

    private void UpdateText(int value)
    {
        text.text = $"$ {value}";
    }
}