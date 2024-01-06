using System;
using UnityEngine;

namespace RPG.Adventure.Player
{
    [Serializable]
    public class PlayerRunProperty
    {
        [SerializeField, Tooltip("Run時の速度")]
        private float _moveSpeed = 5.0F;

        [SerializeField, Tooltip("プレイヤーのターンの速度")]
        private float _turnSpeed = 0.5F;

        /// <summary>Walk時の速度</summary>
        public float MoveSpeed => _moveSpeed;

        /// <summary>プレイヤーのターンの速度</summary>
        public float TurnSpeed => _turnSpeed;
    }
}
