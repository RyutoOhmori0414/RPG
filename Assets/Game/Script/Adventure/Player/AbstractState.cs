using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public abstract class AbstractState
    {
        protected StateConditions _conditions;
        
        
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
