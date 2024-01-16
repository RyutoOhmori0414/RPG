using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CommonStateMachine;

namespace RPG.Adventure.Player
{
    public class PlayerStateMachine : AbstractSingleStateMachine<AbstractPlayerState>
    {
        [SerializeField, Tooltip("プレイヤーのProperty")]
        private PlayerProperty _property = default;

        /// <summary>プレイヤーのProperty</summary>
        public PlayerProperty PlayerProperty => _property;

        /// <summary>アニメーションのトリガーを入れるためのクラス</summary>
        public PlayerAnimParams AnimParams { get; private set; }

        private void Awake()
        {
            _property.Init(this);
            
            // キャッシュの初期化
            StateCache<PlayerIdleState>.cache = new PlayerIdleState(_property);
            StateCache<PlayerWalkState>.cache = new PlayerWalkState(_property);
            StateCache<PlayerRunState>.cache = new PlayerRunState(_property);
            StateCache<PlayerAttackState>.cache = new PlayerAttackState(_property);
            
            // AnimParamの初期化
            AnimParams = new PlayerAnimParams(StateCache<PlayerWalkState>.cache.IsMoveRP,
                StateCache<PlayerRunState>.cache.IsRunRP, StateCache<PlayerAttackState>.cache.IsAttackRP);
        }

        private void Start()
        {
            TransitionState<PlayerIdleState>();
        }
        
        /// <summary>アニメーションイベントで呼ばれる攻撃終了</summary>
        private void AnimationAttackEnd() => StateCache<PlayerAttackState>.cache.AnimationAttackEnd();
    }
}
