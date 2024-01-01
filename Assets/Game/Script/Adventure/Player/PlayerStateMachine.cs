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

        /// <summary>アニメーションのトリガーを入れるためのクラス</summary>
        public PlayerAnimParams AnimParams { get; private set; }

        /// <summary>現在のステート</summary>
        private AbstractState _currentState = null;

        private void Awake()
        {
            _property.Init(this);
            
            // キャッシュの初期化
            StateCache<PlayerIdleState>.cache = new PlayerIdleState(_property);
            StateCache<PlayerWalkState>.cache = new PlayerWalkState(_property);
            StateCache<PlayerRunState>.cache = new PlayerRunState(_property);
            StateCache<PlayerAttackState>.cache = new PlayerAttackState(_property);
            
            // AnimPramの初期化
            AnimParams = new PlayerAnimParams(StateCache<PlayerWalkState>.cache.IsMoveRP,
                StateCache<PlayerRunState>.cache.IsRunRP, StateCache<PlayerAttackState>.cache.IsAttackRP);
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
        
        /// <summary>アニメーションイベントで呼ばれる攻撃終了</summary>
        private void AnimationAttackEnd() => StateCache<PlayerAttackState>.cache.AnimationAttackEnd();

        /// <summary>型をKeyにしてStateをCacheしておくクラス</summary>
        /// <typeparam name="T">StateType</typeparam>
        private static class StateCache<T> where T : AbstractState
        {
            /// <summary>キャッシュ</summary>
            public static T cache;
        }
    }
}
