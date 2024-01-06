using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CommonStateMachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPG.Adventure.Enemy
{
    public class EnemyStateMachine : AbstractStateMachine<AbstractEnemyState>
    {
        [SerializeField, Tooltip("EnemyのData")]
        private EnemyPropertyScriptableObject _property = default;

        private void Awake()
        {
            // キャッシュの初期化
            StateCache<EnemySearchState>.cache = new EnemySearchState(_property, this);
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
            else
            {
                (StateCache<EnemySearchState>.cache ??= new (_property, this)).OnDrawGizmo();   
            }
        }
#endif
    }
}