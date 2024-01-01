using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CommonStateMachine;

namespace RPG.Adventure.Enemy
{
    public abstract class AbstractEnemyState : IState
    {
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnLateUpdate();
        public abstract void OnExit();
    }
}