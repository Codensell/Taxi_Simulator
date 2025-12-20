using UnityEngine;

public sealed class DestinationArrow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 4f, 0f);

    private Transform _target;
    private Transform _taxi;

    public void Show(Transform target, Transform taxi)
    {
        _target = target;
        _taxi = taxi;

        gameObject.SetActive(true);

        Debug.Log(
            $"[DestinationArrow] Show | target={(target != null ? target.name : "NULL")} " +
            $"taxi={(taxi != null ? taxi.name : "NULL")}"
        );
    }

    public void Hide()
    {
        Debug.Log("[DestinationArrow] Hide");
        _target = null;
        _taxi = null;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_target == null)
            return;

        transform.position = _target.position + offset;
    }

}