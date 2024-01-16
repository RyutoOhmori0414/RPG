using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CommonStateMachine;
using RPG.Adventure.Player;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPG.Adventure.Enemy
{
    public class EnemyStateMachine : AbstractStateMachine<AbstractEnemyState>
    {
        [SerializeField, Tooltip("EnemyのData")]
        private EnemyPropertyScriptableObject _property = default;
        [FormerlySerializedAs("playerSingle")] [FormerlySerializedAs("_player")] [SerializeField]
        private PlayerStateMachine player = default;
        
        private Animator _animator = default;
        public Animator EnemyAnimator => _animator;
        
        public Transform PlayerTransform => player.transform;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            // キャッシュの初期化
            InitCache(new EnemySearchState(_property, this));
            InitCache(new EnemyChaseState(_property, this));
            InitCache(new EnemyAttackState(_property, this));
        }

        private void Start()
        {
            TransitionState<EnemySearchState>();
        }

        private void OnDestroy()
        {
            CacheClear();
        }

        /// <summary>攻撃終了を伝えるAnimationEvent</summary>
        private void OnAttackEndAnimationEvent() => GetCache<EnemyAttackState>().OnAttackEndAnimationEvent();

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying)
            {
                ((AbstractEnemyState)_currentState).OnDrawGizmo();
            }
            else
            {
                if (TryGetCache(out EnemySearchState cache))
                {
                    cache.OnDrawGizmo();
                }
                else
                {
                    InitCache(new EnemySearchState(_property, this));
                    GetCache<EnemySearchState>().OnDrawGizmo();
                }
                
                
                
                if (TryGetCache(out EnemyChaseState cache2))
                {
                    cache.OnDrawGizmo();
                }
                else
                {
                    InitCache(new EnemyChaseState(_property, this));
                    GetCache<EnemyChaseState>().OnDrawGizmo();
                }
            }
        }
#endif
    }
}