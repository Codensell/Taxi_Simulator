using UnityEngine;

namespace Game.Gameplay.Taxi
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class TaxiMovement : MonoBehaviour
    {
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
            if (_rigidbody.linearVelocity.sqrMagnitude < 0.1f)
                return;

            float turnAmount = _steeringInput * _stats.TurnSpeed * Time.fixedDeltaTime;
            Quaternion rotation = Quaternion.Euler(0f, turnAmount, 0f);

            _rigidbody.MoveRotation(_rigidbody.rotation * rotation);
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

