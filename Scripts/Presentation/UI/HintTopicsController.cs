using TMPro;
using UnityEngine;

namespace Game.UI
{
    public sealed class HintTopicsController : MonoBehaviour
    {
        [SerializeField] private TMP_Text hintContentText;
        [SerializeField] private GameObject contentPanel;


        [Header("Texts")]
        [TextArea] [SerializeField] private string howToRefuel =
            "Заедь на заправку и нажми F, когда появится подсказка. Над заправкой красная канистра!";

        [TextArea] [SerializeField] private string howToPickupPassenger =
            "Подъедь к пассажиру с жёлтой стрелкой над головой и нажми E.\nПоявится полоска - это время за которое нужно успеть доставить пассажира на место!";

        [TextArea] [SerializeField] private string howToChangeCar =
            "Заедь в автосервис. Над автосервисом висит картинка машинки!";

        private void Awake()
        {
            if (contentPanel != null)
                contentPanel.SetActive(false);
        }


        public void ShowRefuel()
        {
            SetText(howToRefuel);
        }

        public void ShowPickupPassenger()
        {
            SetText(howToPickupPassenger);
        }

        public void ShowChangeCar()
        {
            SetText(howToChangeCar);
        }

        private void SetText(string value)
        {
            if (contentPanel != null)
                contentPanel.SetActive(true);

            if (hintContentText != null)
                hintContentText.text = value;
        }

    }
}