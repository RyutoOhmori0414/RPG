using UnityEngine;
using VContainer;
using MessagePipe;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace RPG.Adventure.Input
{
    public class PlayerAdventureInputProvider : IInitializable
    {
        /// <summary>PlayerAdventureInputを送るためのPublisher</summary>
        [Inject]
        private IPublisher<PlayerAdventureInput> _inputPublisher;
        
        /// <summary>PlayerのInputMap</summary>
        private PlayerInputMap _playerInputMap;

        /// <summary>現在のMoveの入力</summary>
        private Vector2 _currentMove = Vector3.zero;

        /// <summary>現在のIsRunInput</summary>
        private bool _currentIsRunInput = false;

        /// <summary>現在のIsDecideInput</summary>
        private bool _currentIsDecideInput = false;
        
        public void Initialize()
        {
            // InputMapの初期化
            _playerInputMap = new();
            
            _playerInputMap.Enable();
            _playerInputMap.Adventure.Enable();
            
            // 関数の登録
            _playerInputMap.Adventure.Move.started += InputMoveUpdate;
            _playerInputMap.Adventure.Move.performed += InputMoveUpdate;
            _playerInputMap.Adventure.Move.canceled += InputMoveUpdate;

            _playerInputMap.Adventure.Run.started += InputRunUpdate;
            _playerInputMap.Adventure.Run.canceled += InputRunUpdate;

            _playerInputMap.Adventure.Deside.started += InputDecideUpdate;
            _playerInputMap.Adventure.Deside.canceled += InputDecideUpdate;
        }

        /// <summary>Moveを更新する関数</summary>
        /// <param name="callback">コールバック</param>
        private void InputMoveUpdate(InputAction.CallbackContext callback)
        {
            if (callback.canceled)
            {
                _currentMove = Vector2.zero;
            }
            else
            {
                _currentMove = callback.ReadValue<Vector2>();
            }
            
            Publish();
        }

        /// <summary>RunInputを更新する関数</summary>
        /// <param name="callback">コールバック</param>
        private void InputRunUpdate(InputAction.CallbackContext callback)
        {
            
            if (callback.canceled)
            {
                _currentIsRunInput = false;
            }
            else
            {
                _currentIsRunInput = true;
            }
            
            Publish();
        }
        
        /// <summary>DecideInputを更新する関数</summary>
        /// <param name="callback">コールバック</param>
        private void InputDecideUpdate(InputAction.CallbackContext callback)
        {
            if (callback.canceled)
            {
                _currentIsDecideInput = false;
            }
            else
            {
                _currentIsDecideInput = true;
            }
            
            Publish();
        }
        
        

        private void Publish()
        {
            var inputData = new PlayerAdventureInput(_currentMove, _currentIsRunInput, _currentIsDecideInput);
            
            _inputPublisher.Publish(inputData);
        }

        ~PlayerAdventureInputProvider()
        {
            _playerInputMap.Dispose();
        }
    }   
}
