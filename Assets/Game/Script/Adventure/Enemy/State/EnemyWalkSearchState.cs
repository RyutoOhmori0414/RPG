using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure.Enemy
{
    public class EnemyWalkSearchState : AbstractEnemyState
    {
        /// <summary>Playerを発見したかどうか</summary>
        private bool _isFound = false;

        private Vector3 _target = Vector3.zero;

        private CharacterController _characterController;
        
        public EnemyWalkSearchState(EnemyPropertyScriptableObject property, EnemyStateMachine stateMachine) : base(property, stateMachine)
        {
            _characterController = stateMachine.GetComponent<CharacterController>();
        }

        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            Search();
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnLateUpdate()
        {
        }

        public override void OnExit()
        {
            _isFound = false;
        }
        
        public override void OnDrawGizmo()
        {
            Gizmos.color = new(0.0F, 0.0F, 1.0F, 0.3F);
            CustomGizmo.DrawFunGizmo(_stateMachine.transform, _property.Search.SearchAngle, _property.Search.SearchRange);
        }

        private void UpdateTarget()
        {
            _target = _stateMachine.transform.position +
                      new Vector3(Random.Range(-7.0F, 7.0F), 0, Random.Range(-7.0F, 7.0F));
        }
        
        private void Walk()
        {
            var dir = _target - _stateMachine.transform.position;
            dir.Normalize();

            dir *= _property.Search.SearchWalkSpeed;

            dir.y = Physics.gravity.y * Time.deltaTime;

            _characterController.Move(dir * Time.deltaTime);
        }
        
        private void Search()
        {
            var enemyTransform = _stateMachine.transform;
            var playerTransform = _stateMachine.PlayerTransform;

            // 距離判定
            var dir = playerTransform.position - enemyTransform.position;
            var sqrLength = dir.sqrMagnitude;

            if (sqrLength < _property.Search.SearchRange * _property.Search.SearchRange)
            {
                var angle = Vector3.Angle(enemyTransform.forward, dir);

                if (angle < _property.Search.SearchAngle)
                {
                    _isFound = true;
                }
            }
        }
    }   
}
