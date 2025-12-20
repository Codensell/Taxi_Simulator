using UnityEngine;
using System.Collections.Generic;

public sealed class PassengerPickup : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private GameObject view;
    [SerializeField] private GameObject pickupTrigger;
    [SerializeField] private GameObject waitingArrow;

    [Header("Destination")]
    [SerializeField] private Transform pathsRoot;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string waveStateName = "wave";

    public Transform Destination { get; private set; }

    private bool _pickedUp;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private void Awake()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;

        if (animator == null && view != null)
            animator = view.GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        PlayWave();
    }

    public bool CanBePickedUp()
    {
        return !_pickedUp;
    }

    public void Pickup()
    {
        if (_pickedUp)
            return;

        _pickedUp = true;

        // 🔑 ВАЖНО: выбираем destination ЗДЕСЬ
        Destination = ChooseDestination();

        if (waitingArrow != null)
            waitingArrow.SetActive(false);

        if (view != null)
            view.SetActive(false);

        if (pickupTrigger != null)
            pickupTrigger.SetActive(false);
    }

    public void ReturnToStart()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;

        _pickedUp = false;
        Destination = null;

        if (view != null)
            view.SetActive(true);

        if (pickupTrigger != null)
            pickupTrigger.SetActive(true);

        if (waitingArrow != null)
            waitingArrow.SetActive(true);

        PlayWave();
    }

    // ✅ ВОТ ОН — КЛЮЧЕВОЙ МЕТОД
    private Transform ChooseDestination()
    {
        if (pathsRoot == null)
        {
            Debug.LogWarning("[PassengerPickup] PathsRoot is NULL");
            return null;
        }

        List<Transform> points = new List<Transform>();

        foreach (Transform path in pathsRoot)
        {
            foreach (Transform p in path)
                points.Add(p);
        }

        if (points.Count == 0)
        {
            Debug.LogWarning("[PassengerPickup] No destination points found");
            return null;
        }

        Transform chosen = points[Random.Range(0, points.Count)];

        Debug.Log($"[PassengerPickup] Destination chosen: {chosen.name}");
        return chosen;
    }

    private void PlayWave()
    {
        if (animator == null)
            return;

        animator.Play(waveStateName, 0, 0f);
        animator.Update(0f);
    }
}
