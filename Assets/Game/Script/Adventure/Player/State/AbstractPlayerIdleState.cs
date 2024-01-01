using MessagePipe;
using RPG.Adventure.Input;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RPG.Adventure.Player
{
    public class AbstractPlayerIdleState : AbstractPlayerState
    {
        public AbstractPlayerIdleState(PlayerProperty property) : base(property)
        {
            // 条件
            _conditions = new StateConditions(
                () =>
                {
                    // Idle -> Walk
                    if (_currentInput.Move != Vector2.zero)
                    {
                        _property.TransitionState<AbstractPlayerWalkState>();
                        return true;
                    }

                    return false;
                },
                () =>
                {
                    // Idle -> Attack
                    if (_currentInput.IsDecideInput)
                    {
                        _property.TransitionState<AbstractPlayerAttackState>();
                        return true;
                    }

                    return false;
                }
            );
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
    }   
}