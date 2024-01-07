using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure.Enemy
{
    public class EnemyChaseState : AbstractEnemyState
    {
        private bool _isFound = false;

        private CharacterController _characterController;
        
        public EnemyChaseState(EnemyPropertyScriptableObject property, EnemyStateMachine stateMachine) : base(property, stateMachine)
        {
            _conditions = new(
                () =>
                {
                    if (!_isFound)
                    {
                        stateMachine.TransitionState<EnemySearchState>();
                        return true;
                    }

                    return false;
                }
                );
            _characterController = _stateMachine.GetComponent<CharacterController>();
        }

        public override void OnEnter()
        {
            _isFound = true;
        }

        public override void OnUpdate()
        {
            Chase();

            _conditions.Check();
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnLateUpdate()
        {
        }

        public override void OnExit()
        {
        }

        private void Chase()
        {
            var enemyTransform = _stateMachine.transform;
            var playerTransform = _stateMachine.PlayerTransform;

            var dir = playerTransform.position - enemyTransform.position;
            dir.y = 0.0F;
            
            Move(dir);
            Rotate(dir, enemyTransform);
            CheckDistance(enemyTransform.position, playerTransform.position);
        }

        private void Move(Vector3 dir)
        {
            dir.Normalize();
            dir *= _property.Chase.ChaseSpeed;
            dir.y = Physics.gravity.y * Time.deltaTime;

            _characterController.Move(dir * Time.deltaTime);
        }

        private void Rotate(Vector3 dir, Transform enemyTransform)
        {
            enemyTransform.forward = dir;
        }

        private void CheckDistance(Vector3 pos1, Vector3 pos2)
        {
            var sqrDistance = (pos1 - pos2).sqrMagnitude;

            if (sqrDistance > _property.Chase.MissRange * _property.Chase.MissRange)
            {
                _isFound = false;
            }
        }
    }
}
