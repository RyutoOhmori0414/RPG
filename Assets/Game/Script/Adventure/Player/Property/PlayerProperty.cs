using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using RPG.Adventure.Input;
using VContainer;

namespace RPG.Adventure.Player
{
    [Serializable]
    public class PlayerProperty
    {
        /// <summary>プレイヤーのステートマシーン</summary>
        private PlayerStateMachine _stateMachine = null;

        /// <summary>プレイヤーのステートマシーン</summary>
        public PlayerStateMachine StateMachine
        {
            set => _stateMachine = value;
        }

        /// <summary>PlayerのTransform</summary>
        public Transform PlayerTransform => _stateMachine.transform;

        /// <summary>Inputの更新を受け取るためのSubscriber</summary>
        [Inject]
        private ISubscriber<PlayerAdventureInput> _inputSubscriber = default;

        /// <summary>Inputの更新を受け取るためのSubscriber</summary>
        public ISubscriber<PlayerAdventureInput> InputSubscriber => _inputSubscriber;

        /// <summary>PlayerからGetComponentする</summary>
        /// <typeparam name="T">取得したい型</typeparam>
        /// <returns>アタッチされたInstance</returns>
        public T GetComponentFromPlayer<T>() => _stateMachine.GetComponent<T>();

        /// <summary>ステートを遷移させる</summary>
        /// <typeparam name="T">遷移させるState</typeparam>
        public void TransitionState<T>() where T : AbstractState=> _stateMachine.TransitionState<T>();

        /// <summary>PlayerのGetCancellationTokenOnDestroy()を取得する</summary>
        /// <returns></returns>
        public CancellationToken GetPlayerCancellationTokenOnDestroy() => _stateMachine.GetCancellationTokenOnDestroy();
    }   
}