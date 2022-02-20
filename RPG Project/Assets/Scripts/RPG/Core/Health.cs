using System;
using RPG.Core;
using Saving;
using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(ActionScheduler), typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float initialHealthPoints = 100f;

        public bool IsDead { get; private set; }
        private float _currentHealthPoints;

        private Animator _animator;
        private static readonly int DeadId = Animator.StringToHash("Dead");

        private void Awake()
        {
            _currentHealthPoints = initialHealthPoints;
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            _currentHealthPoints = Math.Max(_currentHealthPoints - damage, 0);
            // Debug.Log(_currentHealthPoints);
            if (_currentHealthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;

            _animator.SetTrigger(DeadId);
            IsDead = true;

            // TODO set enable to false on enemy capsule collider
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return _currentHealthPoints;
        }

        public void RestoreState(object state)
        {
            if (state is float savedHealthPoints)
            {
                // TODO debug
                print("restore health");
                _currentHealthPoints = savedHealthPoints;
            }

            if (_currentHealthPoints == 0)
                Die();
        }
    }
}