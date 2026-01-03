using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public sealed class MusicDucker : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string musicVolumeDbParameter = "MusicVolumeDb";

    [Header("Ducking")]
    [SerializeField] private float duckedDb = -18f;          // насколько утишаем музыку
    [SerializeField] private float fadeDownSeconds = 0.05f;  // быстро вниз
    [SerializeField] private float fadeUpSeconds = 0.20f;    // плавно вверх

    private Coroutine _routine;
    private float _baseDb;
    private bool _baseDbCached;

    private void Awake()
    {
        CacheBaseDb();
    }

    public void DuckForSeconds(float seconds)
    {
        if (audioMixer == null || string.IsNullOrWhiteSpace(musicVolumeDbParameter))
            return;

        if (seconds <= 0f)
            return;

        CacheBaseDb();

        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(DuckRoutine(seconds));
    }

    private void CacheBaseDb()
    {
        if (_baseDbCached)
            return;

        if (audioMixer != null && audioMixer.GetFloat(musicVolumeDbParameter, out float db))
        {
            _baseDb = db;
            _baseDbCached = true;
        }
        else
        {
            _baseDb = 0f;
            _baseDbCached = true;
        }
    }

    private IEnumerator DuckRoutine(float seconds)
    {
        yield return FadeTo(duckedDb, fadeDownSeconds);
        yield return new WaitForSecondsRealtime(seconds);
        yield return FadeTo(_baseDb, fadeUpSeconds);

        _routine = null;
    }

    private IEnumerator FadeTo(float targetDb, float duration)
    {
        if (duration <= 0f)
        {
            audioMixer.SetFloat(musicVolumeDbParameter, targetDb);
            yield break;
        }

        audioMixer.GetFloat(musicVolumeDbParameter, out float startDb);

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / duration);
            float db = Mathf.Lerp(startDb, targetDb, k);
            audioMixer.SetFloat(musicVolumeDbParameter, db);
            yield return null;
        }

        audioMixer.SetFloat(musicVolumeDbParameter, targetDb);
    }
}
