using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;

namespace RPG.Battle.System
{
    public class BattleEventManager : IBattleManager
    {
        private int _currentTurn = 1;
        private BattlePhase _currentPhase = BattlePhase.PlayerCommandPhase;
        
        private readonly IPublisher<PhaseParams> _phasePublisher;

        public BattleEventManager(IPublisher<PhaseParams> phasePublisher)
        {
            _phasePublisher = phasePublisher;
            _phasePublisher.Publish(new PhaseParams(_currentTurn, _currentPhase, true));
        }

        public void PhaseEnd()
        {
            var isTurnChanged = false;
            _currentPhase++;
            if (_currentPhase > BattlePhase.SlipDamage)
            {
                _currentPhase = BattlePhase.PlayerCommandPhase;
                _currentTurn++;
                isTurnChanged = true;
            }

            var message = new PhaseParams(_currentTurn, _currentPhase, isTurnChanged);
            _phasePublisher.Publish(message);
        }
    }   
}
