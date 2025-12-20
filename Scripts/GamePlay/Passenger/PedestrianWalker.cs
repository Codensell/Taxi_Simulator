using UnityEngine;

public sealed class PedestrianWalker : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float reachDistance = 0.2f;
    [SerializeField] private float turnSpeed = 8f;
    [SerializeField] private float turnThreshold = 2f;

    private int _index;
    private bool _isTurning;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (points == null || points.Length == 0)
            SetWalk(false);
    }

    private void LateUpdate()
    {
        if (points == null || points.Length == 0)
            return;

        Move();
    }

    private void Move()
    {
        Transform target = points[_index];
        Vector3 to = target.position - transform.position;
        to.y = 0f;

        if (!_isTurning && to.sqrMagnitude <= reachDistance * reachDistance)
        {
            _index = (_index + 1) % points.Length;
            _isTurning = true;
            SetWalk(false);
            return;
        }

        Vector3 dir = to.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);

        if (_isTurning)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );

            float angle = Quaternion.Angle(transform.rotation, targetRotation);
            if (angle <= turnThreshold)
                _isTurning = false;

            return;
        }
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );

        transform.position +=
            transform.forward * speed * Time.deltaTime;

        SetWalk(true);
    }

    private void SetWalk(bool value)
    {
        if (_animator != null)
            _animator.SetBool("walk", value);
    }
}
