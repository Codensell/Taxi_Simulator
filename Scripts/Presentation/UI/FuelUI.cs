using UnityEngine;
using UnityEngine.UI;

public sealed class FuelUI : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        slider.value = 1f;
    }

    private void Start()
    {
        if (session == null || session.FuelTank == null)
        {
            Debug.LogError("[FuelUI] GameSession or FuelTank is NULL");
            return;
        }

        session.FuelTank.OnFuelChanged += OnFuelChanged;

        OnFuelChanged(session.FuelTank.CurrentFuel, session.FuelTank.MaxFuel);
    }

    private void OnDestroy()
    {
        if (session?.FuelTank == null)
            return;

        session.FuelTank.OnFuelChanged -= OnFuelChanged;
    }

    private void OnFuelChanged(float current, float max)
    {
        float normalized = max > 0f ? current / max : 0f;
        slider.value = normalized;

        if (normalized > 0.5f)
        {
            float t = (normalized - 0.5f) / 0.5f;
            fillImage.color = Color.Lerp(Color.yellow, Color.green, t);
        }
        else
        {
            float t = normalized / 0.5f;
            fillImage.color = Color.Lerp(Color.red, Color.yellow, t);
        }
    }
}