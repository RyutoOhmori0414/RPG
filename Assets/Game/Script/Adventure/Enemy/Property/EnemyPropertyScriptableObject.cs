using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure.Enemy
{
    [CreateAssetMenu(fileName = "AdventureEnemyData", menuName = "ScriptableObject/EnemyPropertyScriptableObject", order = 1)]
    public class EnemyPropertyScriptableObject : ScriptableObject
    {
        [SerializeField]
        private EnemyChaseProperty _chase = new EnemyChaseProperty();

        public EnemyChaseProperty Chase => _chase;
    }
}