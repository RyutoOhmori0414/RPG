using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace RPG.Adventure.Player
{
    public class PlayerAnimParams
    {
        public PlayerAnimParams(IReadOnlyReactiveProperty<bool> isMove, 
            IReadOnlyReactiveProperty<bool> isRunning, IReadOnlyReactiveProperty<bool> isAttack)
        {
            IsMove = isMove;
            IsRunning = isRunning;
            IsAttack = isAttack;
        }
        
        public IReadOnlyReactiveProperty<bool> IsMove { get; }
        
        public IReadOnlyReactiveProperty<bool> IsRunning { get; }
        
        public IReadOnlyReactiveProperty<bool> IsAttack { get; }
    }   
}
