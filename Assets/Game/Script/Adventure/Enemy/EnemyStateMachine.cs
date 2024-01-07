using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CommonStateMachine;
using RPG.Adventure.Player;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPG.Adventure.Enemy
{
    public class EnemyStateMachine : AbstractStateMachine<AbstractEnemyState>
    {
        [SerializeField, Tooltip("EnemyのData")]
        private EnemyPropertyScriptableObject _property = default;
        [SerializeField]
        private PlayerStateMachine _player = default;

        public Transform PlayerTransform => _player.transform;

        private void Awake()
        {
            // キャッシュの初期化
            StateCache<EnemySearchState>.cache = new EnemySearchState(_property, this);
            StateCache<EnemyChaseState>.cache = new EnemyChaseState(_property, this);
        }

        private void Start()
        {
            TransitionState<EnemySearchState>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying)
            {
                ((AbstractEnemyState)_currentState).OnDrawGizmo();
            }
        }
#endif
    }
}