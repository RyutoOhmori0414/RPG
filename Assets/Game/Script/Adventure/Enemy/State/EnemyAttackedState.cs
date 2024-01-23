using System.Collections;
using System.Collections.Generic;
using RPG.CommonStateMachine;
using UnityEngine;

namespace RPG.Adventure.Enemy
{
    public class EnemyAttackedState : AbstractEnemyState
    {
        public EnemyAttackedState(EnemyPropertyScriptableObject property, EnemyStateMachine stateMachine) : base(property, stateMachine)
        {
            _conditions = new ();
        }

        public override void OnEnter()
        {
            _stateMachine.AdventureManager.TransitionToBattle(IAdventureManager.ToBattleAdvantage.None);
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

        public void OnAttackedAnimationEvent()
        {
            _stateMachine.AdventureManager.TransitionToBattle(IAdventureManager.ToBattleAdvantage.None);
        }
    }   
}
