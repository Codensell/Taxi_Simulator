using UnityEngine;

[RequireComponent(typeof(CarWaypointMover))]
[RequireComponent(typeof(CarObstacleDetector))]
public sealed class CarAIController : MonoBehaviour
{
    private CarWaypointMover _mover;
    private CarObstacleDetector _detector;

    private void Awake()
    {
        _mover = GetComponent<CarWaypointMover>();
        _detector = GetComponent<CarObstacleDetector>();
    }

    private void Update()
    {
        _mover.CanMove = !_detector.HasObstacle;
    }

}