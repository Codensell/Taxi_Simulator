using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class FuelStationUI : MonoBehaviour
{
    public event Action<int> OnOptionPayClicked;

    [SerializeField] private TMP_Text[] optionLabels;
    [SerializeField] private Button[] payButtons;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private float warningShowSeconds = 1.5f;

    private float _warningTimer;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (warningText != null)
            warningText.gameObject.SetActive(false);

        BindButtons();
    }

    private void Update()
    {
        if (warningText == null || !warningText.gameObject.activeSelf)
            return;

        _warningTimer -= Time.deltaTime;
        if (_warningTimer <= 0f)
            warningText.gameObject.SetActive(false);
    }

    public void SetOptionsText(string option0, string option1, string option2)
    {
        if (optionLabels == null || optionLabels.Length < 3)
            return;

        optionLabels[0].text = option0;
        optionLabels[1].text = option1;
        optionLabels[2].text = option2;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    public void ShowNotEnoughMoney()
    {
        if (warningText == null)
            return;

        warningText.text = "Не хватает денег!";
        warningText.color = Color.red;
        warningText.gameObject.SetActive(true);
        _warningTimer = warningShowSeconds;
    }

    private void BindButtons()
    {
        if (payButtons == null)
            return;

        for (int i = 0; i < payButtons.Length; i++)
        {
            int index = i;

            if (payButtons[index] == null)
                continue;

            payButtons[index].onClick.AddListener(() => OnOptionPayClicked?.Invoke(index));
        }
    }
}
