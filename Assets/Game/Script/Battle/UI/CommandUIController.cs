using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using RPG.Battle.System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using VContainer;

namespace RPG.Battle.UI
{
    public class CommandUIController : MonoBehaviour
    {
        [SerializeField]
        private InputAction _aButton;
        [SerializeField]
        private UnityEvent _aButtonEvent;
        [SerializeField]
        private InputAction _bButton;
        [SerializeField]
        private UnityEvent _bButtonEvent;
        [SerializeField]
        private InputAction _xButton;
        [SerializeField]
        private UnityEvent _xButtonEvent;
        [SerializeField]
        private InputAction _yButton;
        [SerializeField]
        private UnityEvent _yButtonEvent;

        [Inject]
        private ISubscriber<PhaseParams> _subscriber;
        
        private void Awake()
        {
            _aButton.started += _ => _aButtonEvent.Invoke();
            _bButton.started += _ => _bButtonEvent.Invoke();
            _xButton.started += _ => _xButtonEvent.Invoke();
            _yButton.started += _ => _yButtonEvent.Invoke();
        }

        private void OnEnable()
        {
            _aButton.Enable();
            _bButton.Enable();
            _xButton.Enable();
            _yButton.Enable();
        }

        private void OnDisable()
        {
            _aButton.Disable();
            _bButton.Disable();
            _xButton.Disable();
            _yButton.Disable();
        }

        private void OnDestroy()
        {
            _aButton.Dispose();
            _bButton.Dispose();
            _xButton.Dispose();
            _yButton.Dispose();
        }
    }   
}