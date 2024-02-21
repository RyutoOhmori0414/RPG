using System;
using UnityEngine;

namespace RPG.Battle
{
    public interface IDamage
    {
        public void SendDamage(float damage, BattleAttribute type);

        public void SendHeal(float healHp);

        public Transform Transform { get; }
    }
    
    /// <summary>battleで使用する属性</summary>
    [Serializable]
    public enum BattleAttribute
    {
        None,
        Normal,
        Fire,
        Ice,
        Thunder,
        Wind
    }
}