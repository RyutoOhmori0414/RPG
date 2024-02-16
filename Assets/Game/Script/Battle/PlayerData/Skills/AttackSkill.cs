using System;
using UnityEngine;

namespace RPG.Battle.Player
{
    [Serializable]
    public class AttackSkill : IPlayerSkill
    {
        [SerializeField]
        private string _skillName = "Fire";
        public string SkillName => _skillName;

        [SerializeField]
        private int _requiredMP = 10;
        public int RequiredMP => _requiredMP;

        [SerializeField]
        private float _attackMultiplier = 1.3F;

        public float AttackMultiplier => _attackMultiplier;
    
        [SerializeField] private BattleAttribute _type = BattleAttribute.Fire;

        public BattleAttribute Type => _type;
    }
}