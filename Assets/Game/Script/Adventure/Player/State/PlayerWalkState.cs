using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure.Player
{
    public class PlayerWalkState : AbstractState
    {
        public PlayerWalkState(PlayerProperty property) : base(property)
        {
        }
        
        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            _conditions.Check();
        }

        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnLateUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}
