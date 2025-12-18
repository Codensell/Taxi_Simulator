using UnityEngine;

namespace Game.Presentation.Camera
{
    public sealed class ThirdPersonCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [Header("Offset")]
        [SerializeField] private Vector3 _offset = new Vector3(0f, 4f, -12f);

        [Header("Follow")]
        [SerializeField] private float _positionSmoothTime = 0.15f;
        [SerializeField] private float _rotationSmoothTime = 0.25f;

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
            Vector3 flatForward = _target.forward;
            flatForward.y = 0f;

            if (flatForward.sqrMagnitude < 0.001f)
                return;

            Quaternion desiredRotation = Quaternion.LookRotation(flatForward);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                _rotationSmoothTime * Time.deltaTime * 10f
            );
        }
    }
}