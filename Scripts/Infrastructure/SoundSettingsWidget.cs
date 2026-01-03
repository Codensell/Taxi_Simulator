using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public sealed class SoundSettingsWidget : MonoBehaviour
{
    private const string MusicKey = "audio.music01";
    private const string SfxKey = "audio.sfx01";
    private const float MinDb = -80f;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string musicVolumeDbParameter;
    [SerializeField] private string sfxVolumeDbParameter;

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);

        Load();
        Apply();
        Bind();
    }

    public void TogglePanel()
    {
        if (panel == null)
            return;

        panel.SetActive(!panel.activeSelf);
    }

    private void Bind()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(_ => OnChanged());

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(_ => OnChanged());
    }

    private void OnChanged()
    {
        Apply();
        Save();
    }

    private void Apply()
    {
        if (audioMixer == null)
            return;

        if (musicSlider != null)
            SetMixerVolume01(musicVolumeDbParameter, musicSlider.value);

        if (sfxSlider != null)
            SetMixerVolume01(sfxVolumeDbParameter, sfxSlider.value);
    }

    private void SetMixerVolume01(string parameter, float value01)
    {
        if (string.IsNullOrWhiteSpace(parameter))
            return;

        float v = Mathf.Clamp01(value01);
        float db = v <= 0.0001f ? MinDb : Mathf.Log10(v) * 20f;

        audioMixer.SetFloat(parameter, db);
    }

    private void Load()
    {
        if (musicSlider != null)
            musicSlider.value = PlayerPrefs.GetFloat(MusicKey, 1f);

        if (sfxSlider != null)
            sfxSlider.value = PlayerPrefs.GetFloat(SfxKey, 1f);
    }

    private void Save()
    {
        if (musicSlider != null)
            PlayerPrefs.SetFloat(MusicKey, musicSlider.value);

        if (sfxSlider != null)
            PlayerPrefs.SetFloat(SfxKey, sfxSlider.value);

        PlayerPrefs.Save();
    }
}
