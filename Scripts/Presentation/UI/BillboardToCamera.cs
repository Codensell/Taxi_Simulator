using UnityEngine;

public sealed class BillboardToCamera : MonoBehaviour
{
    [Header("Billboard")]
    [SerializeField] private Camera targetCamera;

    [Header("Float")]
    [SerializeField] private bool enableFloat = true;
    [SerializeField, Min(0f)] private float amplitude = 0.4f; // высота "качания"
    [SerializeField, Min(0.01f)] private float speed = 5.0f;   // скорость "качания"

    private Vector3 _startLocalPosition;

    private void Awake()
    {
        _startLocalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        UpdateBillboard();
        UpdateFloat();
    }

    private void UpdateBillboard()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null)
            return;

        Vector3 forward = cam.transform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.0001f)
            return;

        transform.rotation = Quaternion.LookRotation(forward);
    }

    private void UpdateFloat()
    {
        if (!enableFloat || amplitude <= 0f)
            return;

        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = _startLocalPosition + Vector3.up * offsetY;
    }
}