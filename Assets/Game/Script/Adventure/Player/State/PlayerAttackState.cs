using RPG.CommonStateMachine;
using UniRx;

namespace RPG.Adventure.Player
{
    public class PlayerAttackState : AbstractPlayerState
    {
        /// <summary>Stateの入出を通知するReactiveProperty</summary>
        private readonly ReactiveProperty<bool> _isAttackRP = new ReactiveProperty<bool>();

        /// <summary>Stateの入出を通知するReactiveProperty</summary>
        public IReadOnlyReactiveProperty<bool> IsAttackRP => _isAttackRP;
        
        public PlayerAttackState(PlayerProperty property) : base(property)
        {
            _conditions = new StateConditions(
                () =>
                {
                    if (!_isAttackRP.Value)
                    {
                        _property.TransitionState<PlayerIdleState>();
                        return true;
                    }
                    
                    return false;
                }
            );
        }

        public override void OnEnter()
        {
            _isAttackRP.Value = true;
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
            _isAttackRP.Value = false;
        }

        /// <summary>アニメーションイベントで呼ばれる攻撃終了</summary>
        public void AnimationAttackEnd() => _isAttackRP.Value = false;
    }   
}
