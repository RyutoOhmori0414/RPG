using System;

namespace RPG.Battle.Player
{
    public interface IPlayerSkill
    {
        public string SkillName { get; }

        public int RequiredMP { get; }
    
        public BattleAttribute Type { get; }
    }
}