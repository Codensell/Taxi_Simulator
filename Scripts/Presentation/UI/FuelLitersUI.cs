using System.Collections;
using TMPro;
using UnityEngine;

public sealed class FuelLitersUI : MonoBehaviour
{
    [SerializeField] private GameSession gameSession;
    [SerializeField] private TMP_Text litersText;
    [SerializeField] private bool showDecimals = false;

    private bool _subscribed;

    private void Awake()
    {
        // Чтобы сразу было видно, что компонент жив
        if (litersText != null && string.IsNullOrEmpty(litersText.text))
            litersText.text = "Литров: ...";
    }

    private void OnEnable()
    {
        StartCoroutine(InitWhenReady());
    }

    private void OnDisable()
    {
        Unsubscribe();
        StopAllCoroutines();
    }

    private IEnumerator InitWhenReady()
    {
        // Ждём пока GameSession и FuelTank станут доступны
        while (gameSession == null || gameSession.FuelTank == null)
            yield return null;

        Subscribe();
        Refresh();
    }

    private void Subscribe()
    {
        if (_subscribed)
            return;

        gameSession.FuelTank.OnFuelChanged += OnFuelChanged;
        _subscribed = true;
    }

    private void Unsubscribe()
    {
        if (!_subscribed)
            return;

        if (gameSession != null && gameSession.FuelTank != null)
            gameSession.FuelTank.OnFuelChanged -= OnFuelChanged;

        _subscribed = false;
    }

    private void Refresh()
    {
        var tank = gameSession.FuelTank;
        OnFuelChanged(tank.CurrentFuel, tank.MaxFuel);
    }

    private void OnFuelChanged(float current, float max)
    {
        if (litersText == null)
            return;

        litersText.gameObject.SetActive(true);

        if (showDecimals)
            litersText.text = $"Литров: {current:0.0} / {max:0}";
        else
            litersText.text = $"Литров: {Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}