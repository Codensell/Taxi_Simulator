using UnityEngine;
using Game.Gameplay.Taxi;

[RequireComponent(typeof(TaxiMovement))]
public sealed class TaxiEngineAudio : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private TaxiMovement movement;

    [Header("Sources")]
    [SerializeField] private AudioSource idleSource;
    [SerializeField] private AudioSource driveSource;

    [Header("Mix")]
    [SerializeField, Min(0f)] private float moveThreshold = 0.25f;
    [SerializeField, Min(0.01f)] private float fadeSpeed = 6f;

    [Header("Pitch")]
    [SerializeField] private float idlePitch = 1.0f;
    [SerializeField] private float drivePitchMin = 1.0f;
    [SerializeField] private float drivePitchMax = 1.35f;

    private void Awake()
    {
        if (movement == null)
            movement = GetComponent<TaxiMovement>();

        PrepareSource(idleSource);
        PrepareSource(driveSource);

        if (idleSource != null && !idleSource.isPlaying)
            idleSource.Play();

        if (driveSource != null && !driveSource.isPlaying)
            driveSource.Play();

        if (idleSource != null) idleSource.volume = 1f;
        if (driveSource != null) driveSource.volume = 0f;
    }

    private void Update()
    {
        if (movement == null)
            return;

        float speedAbs = Mathf.Abs(movement.CurrentSpeed);

        float driveTarget = speedAbs >= moveThreshold ? 1f : 0f;
        float idleTarget = 1f - driveTarget;

        if (idleSource != null)
            idleSource.volume = Mathf.MoveTowards(idleSource.volume, idleTarget, fadeSpeed * Time.deltaTime);

        if (driveSource != null)
            driveSource.volume = Mathf.MoveTowards(driveSource.volume, driveTarget, fadeSpeed * Time.deltaTime);

        float maxSpeed = Mathf.Max(0.001f, movement.MaxSpeed);
        float t = Mathf.Clamp01(speedAbs / maxSpeed);

        if (idleSource != null)
            idleSource.pitch = idlePitch;

        if (driveSource != null)
            driveSource.pitch = Mathf.Lerp(drivePitchMin, drivePitchMax, t);
    }

    private static void PrepareSource(AudioSource source)
    {
        if (source == null)
            return;

        source.loop = true;
        source.playOnAwake = false;
        source.dopplerLevel = 0f;
    }
}
