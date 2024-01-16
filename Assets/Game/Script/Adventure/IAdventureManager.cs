using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure
{
    public interface IAdventureManager
    {
        /// <summary>battleシーンへの遷移</summary>
        /// <param name="advantage">Battleシーンに進む際のAdvantage</param>
        public void TransitionToBattle(ToBattleAdvantage advantage);
        
        public enum ToBattleAdvantage
        {
            None,
            PlayerAdvantage,
            EnemyAdvantage
        }
    }   
}
