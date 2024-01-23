using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure.Enemy
{
    [System.Serializable]
    public class EnemyAttackProperty
    {
        [SerializeField, Tooltip("攻撃範囲の中心との距離")]
        private float _attackAreaDistance = 3.0F;

        public float AttackAreaDistance => _attackAreaDistance;

        [SerializeField]
        private Vector3 _attackAreaOffset = Vector3.zero;

        public Vector3 AttackAreaOffset => _attackAreaOffset;
        
        [SerializeField]
        private Vector3 _attackAreaSize = Vector3.one;

        public Vector3 AttackAreaSize => _attackAreaSize;
        
        [SerializeField]
        private LayerMask _attackTarget = default;

        public LayerMask AttackTarget => _attackTarget;
    }   
}
