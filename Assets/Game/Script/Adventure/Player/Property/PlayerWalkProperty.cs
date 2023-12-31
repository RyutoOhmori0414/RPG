using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Adventure.Player
{
    [Serializable]
    public class PlayerWalkProperty
    {
        [SerializeField, Tooltip("Walk時の速度")]
        private float _moveSpeed = 3.0F;

        [SerializeField, Tooltip("最高速度になるまでにかかる時間")]
        private float _maxSpeedDuration = 0.6F;

        [SerializeField, Tooltip("プレイヤーのターンの速度")]
        private float _turnSpeed = 0.5F;

        /// <summary>Walk時の速度</summary>
        public float MoveSpeed => _moveSpeed;

        /// <summary>最高速度になるまでにかかる時間</summary>
        public float MaxSpeedDuration => _maxSpeedDuration;

        /// <summary>プレイヤーのターンの速度</summary>
        public float TurnSpeed => _turnSpeed;
    }
}
