using RPG.Adventure.Input;
using UnityEngine;

namespace RPG.Adventure.Player
{
    public class PlayerWalkState : AbstractState
    {
        /// <summary>キャラクターコントローラー</summary>
        private CharacterController _characterController;

        /// <summary>カメラのTransform</summary>
        private Transform _cameraTransform = default;

        /// <summary>このステートに入ってからの経過時間</summary>
        private float _elapsed = 0.0F;
        
        /// <summary>向いていた方向</summary>
        private Quaternion _lastQuaternion = Quaternion.identity;

        /// <summary>向こうとしている方向</summary>
        private Quaternion _currentQuaternion = Quaternion.identity;

        private float _turnTime = 0.0F;

        /// <summary>方向が変わってからたったか</summary>
        private float _rotateUpdateElapsed = 0.0F;

        /// <summary>現在回転中かどうか</summary>
        private bool _isRotating = false;
        
        public PlayerWalkState(PlayerProperty property) : base(property)
        {
            _conditions = new StateConditions(
                () =>
                {
                    // Walk -> Idle
                    if (_currentInput.Move == Vector2.zero && !_isRotating)
                    {
                        _property.TransitionState<PlayerIdleState>();
                        return true;
                    }

                    return false;
                },
                () =>
                {
                    // Walk -> Run
                    if (_currentInput.IsRunInput)
                    {
                        _property.TransitionState<PlayerRunState>();
                        return true;
                    }

                    return false;
                }
            );

            _characterController = property.GetComponentFromPlayer<CharacterController>();
            _cameraTransform = Camera.main.transform;
        }
        
        public override void OnEnter()
        {
            UpdateQuaternion();
        }

        public override void OnUpdate()
        {
            _elapsed += Time.deltaTime;
            
            var moveDir = GetMoveDir();
            MoveOnUpdate(moveDir);
            RotateOnUpdate(moveDir);
            
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
            _elapsed = 0.0F;
            _rotateUpdateElapsed = 0.0F;
        }

        protected override void InputEventReceiver(PlayerAdventureInput current)
        {
            base.InputEventReceiver(current);
            
            UpdateQuaternion();
        }

        /// <summary>カメラの向きを考慮したMoveDirを返す</summary>
        /// <returns>MoveDir</returns>
        private Vector3 GetMoveDir()
        {
            var moveDir = new Vector3(_currentInput.Move.x, 0.0F, _currentInput.Move.y);
            moveDir = _cameraTransform.TransformDirection(moveDir);
            moveDir.y = 0.0F;
            moveDir.Normalize();

            return moveDir;
        }

        /// <summary>移動処理</summary>
        private void MoveOnUpdate(Vector3 moveDir)
        {
            moveDir *= _property.Walk.MoveSpeed * Mathf.Lerp(0, 1, _elapsed / _property.Walk.MaxSpeedDuration);
            moveDir.y = Physics.gravity.y * Time.deltaTime;

            _characterController.Move(moveDir * Time.deltaTime);
        }

        /// <summary>方向転換</summary>
        private void RotateOnUpdate(Vector3 moveDir)
        {
            _rotateUpdateElapsed += Time.deltaTime;

            _property.PlayerTransform.rotation =
                Quaternion.Slerp(_lastQuaternion, _currentQuaternion, _rotateUpdateElapsed / _turnTime);

            if (_rotateUpdateElapsed / _turnTime >= 1)
            {
                _isRotating = false;
            }
        }

        /// <summary>角度を更新する</summary>
        private void UpdateQuaternion()
        {
            if (_currentInput.Move == Vector2.zero) return;
            
            var moveDir = GetMoveDir();

            _lastQuaternion = _property.PlayerTransform.rotation;
            _currentQuaternion = Quaternion.LookRotation(moveDir);

            var diff = Mathf.Abs(_lastQuaternion.eulerAngles.y - _currentQuaternion.eulerAngles.y);
            _turnTime = (diff / 360.0F) * _property.Walk.TurnSpeed;

            _rotateUpdateElapsed = 0.0F;

            _isRotating = true;
        }
    }
}
