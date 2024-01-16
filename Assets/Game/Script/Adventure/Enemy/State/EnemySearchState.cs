using RPG.CommonStateMachine;
using UnityEngine;
using UniRx;

namespace RPG.Adventure.Enemy
{
    public class EnemySearchState : AbstractEnemyState
    {
        /// <summary>このステートでの経過時間</summary>
        private float _elapsed = 0.0F;
        
        /// <summary>Playerを発見したかどうか</summary>
        private bool _isFound = false;

        /// <summary>Playerとの当たり判定のColliderを受け取る</summary>
        private Collider[] _colliders = new Collider[1];
        
        public EnemySearchState(EnemyPropertyScriptableObject property, EnemyStateMachine stateMachine)
            : base(property, stateMachine)
        {
            _conditions = new StateConditions(() =>
                {
                    if (_elapsed > property.Search.GetStayingTime())
                    {
                        stateMachine.TransitionState<EnemySearchState>();
                        return true;
                    }

                    return false;
                },
                () =>
                {
                    if (_isFound)
                    {
                        Debug.Log("見つけた");
                        stateMachine.TransitionState<EnemyChaseState>();
                        return true;
                    }
                    
                    return false;
                });
        }
        
        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            Search();

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
            _elapsed = 0.0f;
            _isFound = false;
        }

        public override void OnDrawGizmo()
        {
            Gizmos.color = new(0.0F, 0.0F, 1.0F, 0.3F);
            CustomGizmo.DrawFunGizmo(stateMachine.transform, _property.Search.SearchAngle, _property.Search.SearchRange);
        }

        private void Search()
        {
            var enemyTransform = stateMachine.transform;
            var playerTransform = stateMachine.PlayerTransform;

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