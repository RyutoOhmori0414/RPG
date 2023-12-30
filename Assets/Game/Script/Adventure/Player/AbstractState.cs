using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure.Player
{
    public abstract class AbstractState
    {
        /// <summary>このステートの遷移条件</summary>
        protected StateConditions _conditions;

        /// <summary>PlayerのProperty</summary>
        protected readonly PlayerProperty _property;

        public AbstractState(PlayerProperty property)
        {
            _property = property;
        }
        
        /// <summary>このステートに遷移した際の処理</summary>
        public abstract void OnEnter();
        /// <summary>このステートでのUpdate処理</summary>
        public abstract void OnUpdate();
        /// <summary>このステートでのFixedUpdate処理</summary>
        public abstract void OnFixedUpdate();
        /// <summary>このステートでのLateUpdate処理</summary>
        public abstract void OnLateUpdate();
        /// <summary>このステートから遷移する際の処理</summary>
        public abstract void OnExit();
    }
}
