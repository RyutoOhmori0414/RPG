using System;
using UnityEngine;

namespace RPG.Adventure.Enemy
{
    [Serializable]
    public class EnemyChaseProperty
    {
        [SerializeField, Tooltip("Playerを見失う距離")]
        private float _missRange = 7.0F;
        
        public float MissRange => _missRange;
        
        [SerializeField, Tooltip("追いかける速度")]
        private float _chaseSpeed = 2.0F;

        public float ChaseSpeed => _chaseSpeed;
    }   
}