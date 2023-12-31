using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure.Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField, Tooltip("プレイヤーのProperty")]
        private PlayerProperty _property = default;

        /// <summary>プレイヤーのProperty</summary>
        public PlayerProperty PlayerProperty => _property;

        /// <summary>現在のステート</summary>
        private AbstractState _currentState = null;

        private void Awake()
        {
            _property.Init(this);
            
            // キャッシュの初期化
            StateCache<PlayerIdleState>.cache = new PlayerIdleState(_property);
            StateCache<PlayerWalkState>.cache = new PlayerWalkState(_property);
            StateCache<PlayerRunState>.cache = new PlayerRunState(_property);
        }

        private void Start()
        {
            TransitionState<PlayerIdleState>();
        }

        private void Update() => _currentState.OnUpdate();

        private void FixedUpdate() => _currentState.OnFixedUpdate();

        private void LateUpdate() => _currentState.OnLateUpdate();


        public void TransitionState<T>() where T : AbstractState
        {
            _currentState?.OnExit();
            _currentState = StateCache<T>.cache;
            _currentState.OnEnter();
            Debug.Log(nameof(T));
        }

        private static class StateCache<T> where T : AbstractState
        {
            public static T cache;
        }
    }
}
