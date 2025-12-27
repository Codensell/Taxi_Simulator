using UnityEngine;

public sealed class CarWaypointMover : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 6f;
    [SerializeField] private float reachDistance = 0.5f;

    private int _currentIndex;

    public bool CanMove { get; set; } = true;
    private float _fixedY;

    private void Awake()
    {
        _fixedY = transform.position.y;
    }

    private void Update()
    {
        if (!CanMove)
            return;

        if (waypoints == null || waypoints.Length == 0)
            return;

        MoveTowardsWaypoint();
        RotateTowardsWaypoint();
        CheckWaypointReached();
    }

    private void MoveTowardsWaypoint()
    {
        Transform target = waypoints[_currentIndex];

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = target.position;

        // ❗ запрещаем движение по Y
        targetPosition.y = _fixedY;

        Vector3 direction = (targetPosition - currentPosition).normalized;

        Vector3 newPosition = currentPosition + direction * moveSpeed * Time.deltaTime;
        newPosition.y = _fixedY;

        transform.position = newPosition;
    }


    private void RotateTowardsWaypoint()
    {
        Transform target = waypoints[_currentIndex];

        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // ❗ только горизонтальный поворот

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }


    private void CheckWaypointReached()
    {
        Transform target = waypoints[_currentIndex];
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= reachDistance)
        {
            _currentIndex++;

            if (_currentIndex >= waypoints.Length)
                _currentIndex = 0;
        }
    }
}