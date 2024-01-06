using Cysharp.Threading.Tasks;
using MessagePipe;
using RPG.Adventure.Input;
using RPG.CommonStateMachine;
using UnityEngine;

namespace RPG.Adventure.Player
{
    public abstract class AbstractPlayerState : IState
    {
        /// <summary>このステートの遷移条件</summary>
        protected StateConditions _conditions;

        /// <summary>PlayerのProperty</summary>
        protected readonly PlayerProperty _property;

        /// <summary>現在の入力</summary>
        protected PlayerAdventureInput _currentInput = default;

        public AbstractPlayerState(PlayerProperty property)
        {
            _property = property;

            property.InputSubscriber.Subscribe(InputEventReceiver)
                .AddTo(property.GetPlayerCancellationTokenOnDestroy());
        }
        
        /// <summary>このステートに遷移した際の処理</summary>
        public abstract void OnEnter();
        /// <summary>このステートでのUpdate処理</summary>
        public abstract void OnUpdate();
        /// <summary>このステートでのFixedUpdate処理</summary>
        public abstract void OnFixedUpdate();
        /// <summary>このステートでのLateUpdate処理</summary>
        public abstract void OnLateUpdate();
        /// <summary>このステートから遷移する際の処理</summary>
        public abstract void OnExit();
        
        /// <summary>入力を受け取る関数</summary>
        /// <param name="current">入力</param>
        protected virtual void InputEventReceiver (PlayerAdventureInput current)
        {
            _currentInput = current;
        }
    }
}
