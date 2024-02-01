using System.Collections;
using System.Collections.Generic;
using RPG.CommonStateMachine;
using UnityEngine;

namespace RPG.Adventure.Enemy
{
    public class EnemyAttackState : AbstractEnemyState
    {
        private readonly int _attackParamHash = Animator.StringToHash("Attack");

        private bool _isAttacking = false;
        
        public EnemyAttackState(EnemyPropertyScriptableObject property, EnemyStateMachine stateMachine) : base(property, stateMachine)
        {
            _conditions = new(() =>
            {
                if (!_isAttacking)
                {
                    stateMachine.TransitionState<EnemyChaseState>();
                    return true;
                }

                return false;
            });
        }

        public override void OnEnter()
        {
            _isAttacking = true;
            
            // Animatorのフラグをたてる
            _stateMachine.EnemyAnimator.SetTrigger(_attackParamHash);
        }

        public override void OnUpdate()
        {
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

        public override void OnDrawGizmo()
        {
            var enemyTransform = _stateMachine.transform;
            
            Gizmos.color = new Color(1F, 0F, 0F, 0.2F);
            Gizmos.matrix = enemyTransform.localToWorldMatrix;
            Gizmos.DrawCube(
                enemyTransform.forward * _property.Attack.AttackAreaDistance + _property.Attack.AttackAreaOffset,
                _property.Attack.AttackAreaSize);
        }

        /// <summary>攻撃をした際に呼ばれるAnimationEvent</summary>
        public void OnAttackAnimationEvent()
        {
            Attack();
        }

        private void Attack()
        {
            var temp = new Collider[1];
            var enemyTransform = _stateMachine.transform;

            var count = Physics.OverlapBoxNonAlloc(
                enemyTransform.position + enemyTransform.forward * _property.Attack.AttackAreaDistance
                                        + _property.Attack.AttackAreaOffset,
                _property.Attack.AttackAreaSize,
                temp,
                enemyTransform.rotation,
                _property.Attack.AttackTarget);

            if (count > 0)
            {
                _stateMachine.AdventureManager.TransitionToBattle(IAdventureManager.ToBattleAdvantage.EnemyAdvantage);
            }
        }

        /// <summary>攻撃終了時に呼ばれるAnimationEvent</summary>
        public void OnAttackEndAnimationEvent()
        {
            _isAttacking = false;
        }
    }   
}
