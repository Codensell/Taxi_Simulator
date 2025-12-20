using UnityEngine;

public sealed class ArrowFloat : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.25f;
    [SerializeField] private float speed = 2f;

    private Vector3 _startLocalPosition;

    private void Awake()
    {
        Rebase();
    }

    public void Rebase()
    {
        _startLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = _startLocalPosition + Vector3.up * offsetY;
    }
}