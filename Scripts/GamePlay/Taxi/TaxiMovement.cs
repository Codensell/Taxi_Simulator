using UnityEngine;

namespace Game.Gameplay.Taxi
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class TaxiMovement : MonoBehaviour
    {
        [SerializeField] private float _brakeForce = 8f;
        [SerializeField] private float _minTurnSpeed = 1.5f;
        [SerializeField] private float _turnPivotOffset = 1.2f;
        [SerializeField] private TaxiStats _stats;
        private Rigidbody _rigidbody;
        private float _throttleInput;
        private float _steeringInput;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            Move();
            Turn();
            Brake();
            LimitSpeed();
        }
        public void SetInput(float throttle, float steering)
        {
            _throttleInput = Mathf.Clamp(throttle, -1f, 1f);
            _steeringInput = Mathf.Clamp(steering, -1f, 1f);
        }

        private void Move()
        {
            Vector3 force = transform.forward * (_throttleInput * _stats.Acceleration);
            _rigidbody.AddForce(force, ForceMode.Acceleration);
        }
        private void Turn()
        {
            Vector3 velocity = _rigidbody.linearVelocity;
            Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);

            float speed = horizontal.magnitude;
            if (speed < 0.1f)
                return;

            float turnStrength =
                _steeringInput *
                _stats.TurnSpeed *
                Time.fixedDeltaTime;

            Quaternion steerRotation = Quaternion.Euler(0f, turnStrength, 0f);

            Vector3 newDirection = steerRotation * horizontal.normalized;
            Vector3 newVelocity = newDirection * speed;

            _rigidbody.linearVelocity = new Vector3(
                newVelocity.x,
                velocity.y,
                newVelocity.z
            );

            Quaternion targetRotation = Quaternion.LookRotation(newDirection);
            _rigidbody.MoveRotation(
                Quaternion.Slerp(
                    _rigidbody.rotation,
                    targetRotation,
                    0.15f
                )
            );
        }
        private void Brake()
        {
            if (Mathf.Abs(_throttleInput) > 0.01f)
                return;

            Vector3 velocity = _rigidbody.linearVelocity;
            Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);

            Vector3 brake = -horizontal.normalized * _brakeForce;
            _rigidbody.AddForce(brake, ForceMode.Acceleration);
        }
        private void LimitSpeed()
        {
            Vector3 velocity = _rigidbody.linearVelocity;
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

            if (horizontalVelocity.magnitude <= _stats.MaxSpeed)
                return;

            Vector3 limited = horizontalVelocity.normalized * _stats.MaxSpeed;
            _rigidbody.linearVelocity = new Vector3(limited.x, velocity.y, limited.z);
        }
    }
}

