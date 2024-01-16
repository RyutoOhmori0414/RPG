using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Adventure.Enemy
{
    [CreateAssetMenu(fileName = "AdventureEnemyData", menuName = "ScriptableObject/EnemyPropertyScriptableObject", order = 1)]
    public class EnemyPropertyScriptableObject : ScriptableObject
    {
        [SerializeField]
        private bool _isUseNavmesh = false;
        public bool IsUseNavmesh => _isUseNavmesh;
        
        [SerializeField]
        private EnemySearchProperty _search = new ();

        public EnemySearchProperty Search => _search;

        [SerializeField]
        private EnemyChaseProperty _chase = new();

        public EnemyChaseProperty Chase => _chase;
    }
}