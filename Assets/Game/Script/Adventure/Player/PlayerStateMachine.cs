using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CommonStateMachine;

namespace RPG.Adventure.Player
{
    public class PlayerStateMachine : AbstractStateMachine<AbstractPlayerState>
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
            StateCache<AbstractPlayerIdleState>.cache = new AbstractPlayerIdleState(_property);
            StateCache<AbstractPlayerWalkState>.cache = new AbstractPlayerWalkState(_property);
            StateCache<AbstractPlayerRunState>.cache = new AbstractPlayerRunState(_property);
            StateCache<AbstractPlayerAttackState>.cache = new AbstractPlayerAttackState(_property);
            
            // AnimPramの初期化
            AnimParams = new PlayerAnimParams(StateCache<AbstractPlayerWalkState>.cache.IsMoveRP,
                StateCache<AbstractPlayerRunState>.cache.IsRunRP, StateCache<AbstractPlayerAttackState>.cache.IsAttackRP);
        }

        private void Start()
        {
            TransitionState<AbstractPlayerIdleState>();
        }
        
        /// <summary>アニメーションイベントで呼ばれる攻撃終了</summary>
        private void AnimationAttackEnd() => StateCache<AbstractPlayerAttackState>.cache.AnimationAttackEnd();
    }
}
