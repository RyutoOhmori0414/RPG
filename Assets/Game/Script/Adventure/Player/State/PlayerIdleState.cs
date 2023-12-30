using MessagePipe;
using RPG.Adventure.Input;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RPG.Adventure.Player
{
    public class PlayerIdleState : AbstractState
    {
        public PlayerIdleState(PlayerProperty property) : base(property)
        {
            _conditions = new StateConditions();
            
            _property.InputSubscriber.Subscribe(OnInputEvent).AddTo(_property.GetPlayerCancellationTokenOnDestroy());
        }
        
        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            _conditions.Check();
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnLateUpdate()
        {
        }

        public override void OnExit()
        {
        }

        private void OnInputEvent(PlayerAdventureInput input)
        {
            Debug.Log(input.Move);
        }
    }   
}