using UnityEngine;

namespace Game.Presentation.Camera
{
    public sealed class ThirdPersonCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [Header("Offset")]
        [SerializeField] private Vector3 _offset = new Vector3(0f, 4f, -12f);

        [Header("Follow")]
        [SerializeField] private float _positionSmoothTime = 0.18f;
        [SerializeField] private float _rotationSmoothTime = 0.12f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            if (_target == null)
                return;

            FollowPosition();
            FollowRotation();
        }

        private void FollowPosition()
        {
            Vector3 desiredPosition =
                _target.position +
                _target.rotation * _offset;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref _velocity,
                _positionSmoothTime
            );
        }

        private void FollowRotation()
        {
            Quaternion desiredRotation = Quaternion.LookRotation(
                _target.forward,
                Vector3.up
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                _rotationSmoothTime
            );
        }
    }
}