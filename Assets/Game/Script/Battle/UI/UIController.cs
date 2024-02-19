using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using RPG.Battle.System;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace RPG.Battle.UI
{
    public class UIController : MonoBehaviour
    {
        [Inject]
        private ISubscriber<PhaseParams> _subscriber;
        [SerializeField]
        private UnityEvent _onInitialize;
        [SerializeField]
        private UnityEvent _onPlayPhaseStart;
        [SerializeField]
        private UnityEvent _onPlayPhaseEnd;

        private void Awake()
        {
            _onInitialize.Invoke();
            destroyCancellationToken.Register(_subscriber.Subscribe(RegisterMethod).Dispose);
        }

        private void RegisterMethod(PhaseParams phaseParams)
        {
            if (phaseParams.CurrentPhase == BattlePhase.PlayerCommandPhase)
            {
                _onPlayPhaseStart.Invoke();
            }
            else if (phaseParams.CurrentPhase == BattlePhase.EnemyAttackPhase)
            {
                _onPlayPhaseEnd.Invoke();
            }
        }
    }   
}
