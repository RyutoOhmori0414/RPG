using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Battle.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private EnemyController[] _enemies;

        public IDamage[] EnemyTargets => _enemies;
    }   
}