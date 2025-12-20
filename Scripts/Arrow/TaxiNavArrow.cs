using UnityEngine;

public sealed class TaxiNavArrow : MonoBehaviour
{
    [SerializeField] private float height = 2.5f;

    private Transform _destination;
    private Transform _taxi;

    private void Awake()
    {
        _taxi = transform.parent;
        gameObject.SetActive(false);
    }

    public void Show(Transform destination)
    {
        _destination = destination;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _destination = null;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (_destination == null || _taxi == null)
            return;

        transform.position = _taxi.position + Vector3.up * height;

        Vector3 dir = _destination.position - _taxi.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        transform.rotation = Quaternion.LookRotation(dir);
    }
}