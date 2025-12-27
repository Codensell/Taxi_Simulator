using UnityEngine;
using UnityEngine.UI;

public sealed class TripTimerUI : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject visualsRoot;


    private void Awake()
    {
        if (visualsRoot == null)
            Debug.LogError("[TripTimerUI] visualsRoot is NULL");
        
        slider.value = 0f;
        visualsRoot.SetActive(false);
    }
    
    private void Start()
    {
        if (session == null)
        {
            Debug.LogError("[TripTimerUI] GameSession is NULL");
            return;
        }

        if (session.TripTimer == null)
        {
            Debug.LogError("[TripTimerUI] TripTimer is NULL");
            return;
        }

        session.TripTimer.OnTimeChanged += OnTimeChanged;
        session.TripTimer.OnExpired += OnExpired;
        session.TripTimer.OnStopped += OnStopped;
        
        if (session.TripTimer.IsRunning)
            OnTimeChanged(session.TripTimer.TimeLeft, session.TripTimer.Duration);
    }

    private void OnDestroy()
    {
        if (session?.TripTimer == null)
            return;

        session.TripTimer.OnTimeChanged -= OnTimeChanged;
        session.TripTimer.OnExpired -= OnExpired;
        session.TripTimer.OnStopped -= OnStopped;
    }

    private void OnTimeChanged(float current, float duration)
    {
        if (!visualsRoot.activeSelf)
            visualsRoot.SetActive(true);

        float normalized = duration > 0f ? current / duration : 0f;
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

    private void OnExpired()
    {
        slider.value = 0f;
        visualsRoot.SetActive(false);
    }
    private void OnStopped()
    {
        slider.value = 0f;
        visualsRoot.SetActive(false);
    }


}