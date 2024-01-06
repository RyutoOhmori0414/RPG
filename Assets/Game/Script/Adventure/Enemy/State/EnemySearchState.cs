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
                    if (_elapsed > property.Chase.GetStayingTime())
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
                        stateMachine.TransitionState<EnemySearchState>();
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
            
        }

        private void Search()
        {
            var halfExtents = new Vector3(_property.Chase.SearchRange, _property.Chase.SearchRange,
                _property.Chase.SearchRange);

            var enemyTransform = _stateMachine.transform;

            var count = Physics.OverlapBoxNonAlloc(enemyTransform.position, halfExtents, _colliders,
                Quaternion.identity, _property.Chase.TargetLayer);

            if (count > 0)
            {
                var dir = _colliders[0].gameObject.transform.position - _stateMachine.transform.position;

                var angle = Vector3.Angle(dir, enemyTransform.forward);

                if (_property.Chase.SearchAngle > angle)
                {
                    _isFound = true;
                }
            }
        }
    }   
}