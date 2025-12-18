using UnityEngine;
using UnityEngine.InputSystem;
using Game.Gameplay.Taxi;

namespace Game.Infrastructure.Input
{
    [RequireComponent(typeof(TaxiMovement))]
    public sealed class KeyboardTaxiInput : MonoBehaviour
    {
        private TaxiMovement _movement;

        private void Awake()
        {
            _movement = GetComponent<TaxiMovement>();
        }

        private void FixedUpdate()
        {
            float throttle = 0f;
            float steering = 0f;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard.wKey.isPressed)
                throttle += 1f;
            if (keyboard.sKey.isPressed)
                throttle -= 1f;

            if (keyboard.aKey.isPressed)
                steering -= 1f;
            if (keyboard.dKey.isPressed)
                steering += 1f;

            _movement.SetInput(throttle, steering);
        }

    }
}