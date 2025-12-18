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

        private Rigidbody _rigidbody;

        private float _throttleInput;
        private float _steeringInput;

        private float _currentSpeed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.constraints =
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationZ;
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

            _rigidbody.MovePosition(_rigidbody.position + delta);
        }

        private void Turn()
        {
            float speedAbs = Mathf.Abs(_currentSpeed);
            if (speedAbs < _minTurnSpeed)
                return;

            float speedFactor = speedAbs / _stats.MaxSpeed;

            float turnAmount =
                _steeringInput *
                _stats.TurnSpeed *
                speedFactor *
                Time.fixedDeltaTime;

            Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
            _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
        }
    }
}

