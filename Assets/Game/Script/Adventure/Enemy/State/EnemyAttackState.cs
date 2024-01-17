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

        /// <summary>攻撃終了時に呼ばれるAnimationEvent</summary>
        public void OnAttackEndAnimationEvent()
        {
            _isAttacking = false;
            _stateMachine.AdventureManager.TransitionToBattle(IAdventureManager.ToBattleAdvantage.EnemyAdvantage);
        }
    }   
}
