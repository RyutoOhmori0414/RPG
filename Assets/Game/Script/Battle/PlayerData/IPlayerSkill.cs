using System;

namespace RPG.Battle.Player
{
    public interface IPlayerSkill
    {
        public string SkillName { get; }

        public int RequiredMP { get; }
    
        public BattleAttribute Type { get; }
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