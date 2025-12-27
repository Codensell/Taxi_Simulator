using UnityEngine;

public sealed class CarObstacleDetector : MonoBehaviour
{
    [SerializeField] private float detectionDistance = 10f;
    [SerializeField] private LayerMask trafficLayer;

    [Header("Performance")]
    [SerializeField, Range(0.05f, 0.5f)]
    private float checkInterval = 1.0f;

    public bool HasObstacle { get; private set; }

    private float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _timer = checkInterval;
            HasObstacle = CheckObstacleAhead();
        }
    }

    private bool CheckObstacleAhead()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = transform.forward;

#if UNITY_EDITOR
        Debug.DrawRay(origin, direction * detectionDistance, Color.yellow);
#endif

        return Physics.Raycast(
            origin,
            direction,
            detectionDistance,
            trafficLayer
        );
    }
}