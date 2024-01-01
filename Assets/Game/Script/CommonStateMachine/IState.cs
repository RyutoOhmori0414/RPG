using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CommonStateMachine
{
    public interface IState
    {
        /// <summary>このステートに遷移した際の処理</summary>
        public void OnEnter();
        /// <summary>このステートでのUpdate処理</summary>
        public void OnUpdate();
        /// <summary>このステートでのFixedUpdate処理</summary>
        public void OnFixedUpdate();
        /// <summary>このステートでのLateUpdate処理</summary>
        public void OnLateUpdate();
        /// <summary>このステートから遷移する際の処理</summary>
        public void OnExit();
    }
}
