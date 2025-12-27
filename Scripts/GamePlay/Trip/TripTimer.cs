using System;

public sealed class TripTimer
{
    public float Duration { get; private set; }
    public float TimeLeft { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<float, float> OnTimeChanged;
    public event Action OnStopped;

    public event Action OnExpired;

    public void Start(float duration)
    {
        Duration = duration;
        TimeLeft = duration;
        IsRunning = true;

        OnTimeChanged?.Invoke(TimeLeft, Duration);
    }

    public void Stop()
    {
        if (!IsRunning)
            return;

        IsRunning = false;
        OnStopped?.Invoke();
    }

    public void Tick(float deltaTime)
    {
        if (!IsRunning)
            return;

        TimeLeft -= deltaTime;

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            IsRunning = false;

            OnTimeChanged?.Invoke(TimeLeft, Duration);
            OnExpired?.Invoke();
            return;
        }

        OnTimeChanged?.Invoke(TimeLeft, Duration);
    }
}