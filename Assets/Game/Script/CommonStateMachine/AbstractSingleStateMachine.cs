using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CommonStateMachine
{
    /// <summary>派生先がシーンで単一な場合使用するStateMachine</summary>
    public abstract class AbstractSingleStateMachine<TStateBase> : MonoBehaviour where TStateBase : IState
    {
        /// <summary>現在のステート</summary>
        protected IState _currentState = null;
        
        protected void Update() => _currentState.OnUpdate();
        protected void FixedUpdate() => _currentState.OnFixedUpdate();
        protected void LateUpdate() => _currentState.OnLateUpdate();
        
        /// <summary>ステートを遷移する関数</summary>
        /// <typeparam name="T">遷移したいState</typeparam>
        public void TransitionState<T>() where T : TStateBase
        {
            _currentState?.OnExit();
            _currentState = StateCache<T>.cache;
            _currentState.OnEnter();
        }

        /// <summary>型をKeyにしてStateをCacheしておくクラス</summary>
        /// <typeparam name="T">StateType</typeparam>
        protected static class StateCache<T> where T : TStateBase
        {
             public static T cache;
        }
    }   
}
