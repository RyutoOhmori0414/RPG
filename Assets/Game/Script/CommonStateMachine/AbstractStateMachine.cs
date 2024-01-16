using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CommonStateMachine
{
    public abstract class AbstractStateMachine<TStateBase> : MonoBehaviour where TStateBase : IState
    {
        /// <summary>現在のステート</summary>
        protected IState _currentState = null;

        private readonly Dictionary<Type, TStateBase> _cache = new();
        
        protected void Update() => _currentState.OnUpdate();
        protected void FixedUpdate() => _currentState.OnFixedUpdate();
        protected void LateUpdate() => _currentState.OnLateUpdate();
        
        /// <summary>ステートを遷移する関数</summary>
        /// <typeparam name="T">遷移したいState</typeparam>
        public void TransitionState<T>() where T : TStateBase
        {
            _currentState?.OnExit();
            _currentState = _cache[typeof(T)];
            _currentState.OnEnter();
        }

        protected void InitCache<T>(T cache) where T : TStateBase
        {
            if (_cache.ContainsKey(typeof(T))) return;
            _cache.Add(typeof(T), cache);
        }

        protected T GetCache<T>() where T : TStateBase
        {
            return (T)_cache[typeof(T)];
        }
        
        protected bool TryGetCache<T>(out T cache) where T : TStateBase
        {
            if (_cache.ContainsKey(typeof(T)))
            {
                cache = (T)_cache[typeof(T)];
                return true;
            }

            cache = default;
            return false;
        }

        protected void CacheClear() => _cache.Clear();
    }   
}