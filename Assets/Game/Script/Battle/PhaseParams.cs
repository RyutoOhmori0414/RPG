using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Battle.System
{
    public struct PhaseParams
    {
        public int CurrentTurn { get; }
        public BattlePhase CurrentPhase { get; }
        
        public bool IsTurnChanged { get; }

        public PhaseParams(int currentTurn, BattlePhase currentPhase, bool isTurnChanged)
        {
            CurrentTurn = currentTurn;
            CurrentPhase = currentPhase;
            IsTurnChanged = isTurnChanged;
        }
    }

    public enum BattlePhase
    {
        PlayerCommandPhase,
        EnemyAttackPhase,
        BuffPhase,
        SlipDamage
    }
}