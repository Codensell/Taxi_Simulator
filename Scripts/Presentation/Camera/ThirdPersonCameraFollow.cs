using UnityEngine;

namespace Game.Presentation.Camera
{
    public sealed class ThirdPersonCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [Header("Offset")]
        [SerializeField] private Vector3 _offset = new Vector3(0f, 4f, -10f);

        [Header("Smooth")]
        [SerializeField] private float _positionSmoothTime = 0.15f;
        [SerializeField] private float _rotationSmoothTime = 0.2f;

        private Vector3 _positionVelocity;

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
                ref _positionVelocity,
                _positionSmoothTime
            );
        }

        private void FollowRotation()
        {
            Quaternion desiredRotation = _target.rotation;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                _rotationSmoothTime
            );
        }
    }
}