using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CommonStateMachine;

namespace RPG.Adventure.Enemy
{
    public abstract class AbstractEnemyState : IState
    {
        /// <summary>遷移条件</summary>
        protected StateConditions _conditions;

        protected readonly EnemyPropertyScriptableObject _property;

        protected readonly EnemyStateMachine _stateMachine;

        public AbstractEnemyState(EnemyPropertyScriptableObject property, EnemyStateMachine stateMachine)
        {
            _property = property;
            _stateMachine = stateMachine;
        }
        
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnLateUpdate();
        public abstract void OnExit();
        
        public virtual void OnDrawGizmo() { }
    }
}