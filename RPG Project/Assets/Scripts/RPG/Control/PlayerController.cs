using System;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Health), typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        private Fighter _fighterSystem;
        private Mover _mover;
        private Health _healthSystem;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighterSystem = GetComponent<Fighter>();
            _healthSystem = GetComponent<Health>();
        }

        private void Update()
        {
            if (_healthSystem.IsDead) return;

            if (InteractWithCombat()) return;

            if (InteractWithMovement()) return;

            print("Nothing to do");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!_fighterSystem.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    _fighterSystem.Attack(target.gameObject);
                }

                return true;
            }

            // hits array doesn't contains any combat targets
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True, if object, under hovering mouse, exists. Otherwise, False.</returns>
        private bool InteractWithMovement()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point);
                }

                // there is object to interact with
                return true;
            }

            return false;
        }

        /// <summary>
        /// Convert mouse position to ray from camera to object under mouse pointer
        /// </summary>
        /// <returns>Ray, where origin: main camera.</returns>
        private static Ray GetMouseRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Debug.DrawRay(ray.origin, ray.direction * 1000);
            return ray;
        }
    }
}