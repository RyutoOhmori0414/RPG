using System;
using UnityEngine;

namespace RPG.Adventure.Enemy
{
    [Serializable]
    public class EnemySearchProperty
    {
        [SerializeField, Tooltip("Enemyの視界距離")]
        private float _searchRange = 5F;
        /// <summary>Enemyの視界距離</summary>
        public float SearchRange => _searchRange / 2;
        
        [SerializeField, Tooltip("Playerを探す範囲")]
        private float _searchAngle = 60F;
        /// <summary>Playerを探す範囲</summary>
        public float SearchAngle => _searchAngle;

        [SerializeField, Tooltip("Playerを探す際のLayerMask")]
        private LayerMask _targetLayer = default;
        /// <summary>Playerを探す際のLayerMask</summary>
        public LayerMask TargetLayer => _targetLayer;

        [SerializeField, Tooltip("このステートの滞在時間をランダムにするのか")]
        private bool _isRandomStayingTime = false;

        [SerializeField, Tooltip("このステートに何秒間いるのか")]
        private float _stayingTime = 3F;

        [SerializeField, Tooltip("滞在時間がランダムの際に使用される最小値")]
        private float _randomStayingMin = 1F;

        [SerializeField, Tooltip("滞在時間がランダムの際に使用される最大値")]
        private float _randomStayingMax = 4f;

        [SerializeField]
        private float _searchWalkSpeed = 2F;
        public float SearchWalkSpeed => _searchWalkSpeed;

        /// <summary>このステートの滞在時間</summary>
        public float GetStayingTime()
        {
            if (_isRandomStayingTime)
            {
                return UnityEngine.Random.Range(_randomStayingMin, _randomStayingMax);
            }

            return _stayingTime;
        }
    }
}