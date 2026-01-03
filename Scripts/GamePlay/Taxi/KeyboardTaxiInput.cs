using UnityEngine;
using UnityEngine.InputSystem;
using Game.Gameplay.Taxi;

namespace Game.Infrastructure.Input
{
    [RequireComponent(typeof(TaxiMovement))]
    public sealed class KeyboardTaxiInput : MonoBehaviour
    {
        private TaxiMovement _movement;

        private float _throttle;
        private float _steering;

        private void Awake()
        {
            _movement = GetComponent<TaxiMovement>();
        }

        private void Update()
        {
            ReadInput(out _throttle, out _steering);
        }

        private void FixedUpdate()
        {
            _movement.SetInput(_throttle, _steering);
        }

        private static void ReadInput(out float throttle, out float steering)
        {
            throttle = 0f;
            steering = 0f;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                throttle += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                throttle -= 1f;

            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                steering -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                steering += 1f;
        }
    }
}