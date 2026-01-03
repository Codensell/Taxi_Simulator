using UnityEngine;

namespace Game.Gameplay.Taxi
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class TaxiMovement : MonoBehaviour
    {
        [SerializeField] private TaxiStats _stats;

        [Header("Driving")]
        [SerializeField] private float _brakeDeceleration = 12f;
        [SerializeField] private float _minTurnSpeed = 0.1f;
        private float _currentTurnVelocity;

        private Rigidbody _rigidbody;

        private float _throttleInput;
        private float _steeringInput;

        private float _currentSpeed;
        [Header("Collision")]
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField, Min(0f)] private float _skinWidth = 0.02f;
        public float CurrentSpeed => _currentSpeed;
        public float MaxSpeed => _stats != null ? _stats.MaxSpeed : 0f;

        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (_boxCollider == null)
                _boxCollider = GetComponent<BoxCollider>();
        }


        private void FixedUpdate()
        {
            UpdateSpeed();
            Move();
            Turn();
        }

        public void SetInput(float throttle, float steering)
        {
            _throttleInput = Mathf.Clamp(throttle, -1f, 1f);
            _steeringInput = Mathf.Clamp(steering, -1f, 1f);
        }

        private void UpdateSpeed()
        {
            float targetSpeed = _throttleInput * _stats.MaxSpeed;

            float accel =
                Mathf.Abs(targetSpeed) > 0.01f
                    ? _stats.Acceleration
                    : _brakeDeceleration;

            _currentSpeed = Mathf.MoveTowards(
                _currentSpeed,
                targetSpeed,
                accel * Time.fixedDeltaTime
            );
        }

        private void Move()
        {
            Vector3 delta =
                transform.forward *
                _currentSpeed *
                Time.fixedDeltaTime;

            if (delta.sqrMagnitude < 0.000001f)
                return;

            Vector3 direction = delta.normalized;
            float distance = delta.magnitude;

            if (_boxCollider == null)
            {
                _rigidbody.MovePosition(_rigidbody.position + delta);
                return;
            }

            Vector3 origin = transform.TransformPoint(_boxCollider.center);

            Vector3 halfExtents = Vector3.Scale(_boxCollider.size, transform.lossyScale) * 0.5f;
            Quaternion orientation = transform.rotation;

            if (Physics.BoxCast(
                    origin,
                    halfExtents,
                    direction,
                    out RaycastHit hit,
                    orientation,
                    distance + _skinWidth,
                    ~0,
                    QueryTriggerInteraction.Ignore))
            {
                float allowed = Mathf.Max(0f, hit.distance - _skinWidth);

                _rigidbody.MovePosition(_rigidbody.position + direction * allowed);
                _currentSpeed = 0f;
                return;
            }

            _rigidbody.MovePosition(_rigidbody.position + delta);
        }


        private void Turn()
        {
            float speedAbs = Mathf.Abs(_currentSpeed);
            if (speedAbs < _minTurnSpeed)
            {
                _currentTurnVelocity = 0f;
                return;
            }

            float speedFactor = Mathf.Clamp01(speedAbs / _stats.MaxSpeed);
            speedFactor *= speedFactor;

            float direction = Mathf.Sign(_currentSpeed);

            float targetTurn =
                _steeringInput *
                direction *
                _stats.TurnSpeed *
                speedFactor;

            _currentTurnVelocity = Mathf.Lerp(
                _currentTurnVelocity,
                targetTurn,
                6f * Time.fixedDeltaTime
            );

            Quaternion turnRotation =
                Quaternion.Euler(0f, _currentTurnVelocity * Time.fixedDeltaTime, 0f);

            _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
        }


    }
}

