using UnityEngine;

public sealed class ArrowTarget : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, 0f);

    public void AttachTo(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("[ArrowTarget] AttachTo called with NULL target");
            return;
        }

        transform.SetParent(target, worldPositionStays: false);

        transform.localPosition = offset;
        transform.localRotation = Quaternion.identity;

        gameObject.SetActive(true);

        if (TryGetComponent<ArrowFloat>(out var arrowFloat))
            arrowFloat.Rebase();

        Debug.Log($"[ArrowTarget] Attached to: {target.name}");
    }
}