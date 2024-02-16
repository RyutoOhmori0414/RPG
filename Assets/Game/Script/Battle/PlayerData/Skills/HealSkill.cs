using System;
using UnityEngine;

namespace RPG.Battle.Player
{
    [Serializable]
    public class HealSkill : IPlayerSkill
    {
        [SerializeField]
        private string _skillName = "Heal";
        public string SkillName => _skillName;

        [SerializeField]
        private int _requiredMP = 10;
        public int RequiredMP => _requiredMP;

        [SerializeField]
        private float _healMultiplier = 1.3F;
        public float HealMultiplier => _healMultiplier;

        [SerializeField]
        private BattleAttribute _type = BattleAttribute.None;
        public BattleAttribute Type => _type;
    }   
}