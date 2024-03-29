using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UnityEngine.Serialization;

namespace RPG.Adventure.Player
{
    public class PlayerAnimController : MonoBehaviour
    {
        [SerializeField, Tooltip("PlayerのAnimator")]
        private Animator _animator = default;

        [FormerlySerializedAs("singleStateMachine")] [FormerlySerializedAs("_stateMachine")] [SerializeField, Tooltip("PlayerStateMachine")]
        private PlayerStateMachine stateMachine = default;

        /// <summary>IsMoveのHash</summary>
        private readonly int _isMoveHash = Animator.StringToHash("IsMove");
        /// <summary>IsRunningのHash</summary>
        private readonly int _isRunningHash = Animator.StringToHash("IsRunning");
        /// <summary>IsAttackのHash</summary>
        private readonly int _isAttackHash = Animator.StringToHash("IsAttack");

        private void Start()
        {
            var param = stateMachine.AnimParams;
            param.IsMove.Subscribe(value => _animator.SetBool(_isMoveHash, value)).AddTo(this);
            param.IsRunning.Subscribe(value => _animator.SetBool(_isRunningHash, value)).AddTo(this);
            param.IsAttack.Subscribe(value => { if (value) _animator.SetTrigger(_isAttackHash); }).AddTo(this);
        }
    }
   
}