using System.Collections;
using System.Collections.Generic;
using RPG.Battle.Player;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace RPG.Battle.Enemy
{
    public class EnemyController : MonoBehaviour, IDamage
    {
        [SerializeField]
        private float _maxHP = 70F;
        private float _currentHP;

        private void Awake()
        {
            _currentHP = _maxHP;
        }

        public void SendDamage(float damage, BattleAttribute type)
        {
            _currentHP -= damage;
        }

        public void SendHeal(float healHp)
        {
            _currentHP += healHp;
        }

        public Transform Transform => transform;
    }   
}