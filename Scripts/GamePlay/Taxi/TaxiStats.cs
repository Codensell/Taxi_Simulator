using UnityEngine;

namespace Game.Gameplay.Taxi
{
    [System.Serializable]
    public sealed class TaxiStats
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _turnSpeed;

        public float MaxSpeed => _maxSpeed;
        public float Acceleration => _acceleration;
        public float TurnSpeed => _turnSpeed;
    }
}