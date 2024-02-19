using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MessagePipe;
using RPG.Battle.System;
using UnityEngine;
using VContainer;

namespace RPG.Battle.Player
{
    public class BattlePlayerController : MonoBehaviour, IDamage
    {
        [SerializeField]
        private string[] _loadSkillNames = default;
        
        [SerializeField]
        private PlayerSkillData _skills = default;

        [SerializeField]
        private float _MaxHP = 100F;

        private float _currentHP;
        
        [Inject]
        private ISubscriber<PhaseParams> _subscriber;
        
        private IPlayerSkill[] _playerSkills;
        public List<IPlayerSkill> PlayerSkills => _playerSkills.ToList();

        public bool IsGuard { get; set; }
        
        private void Awake()
        {
            _playerSkills = Array.ConvertAll(_loadSkillNames, x => _skills.GetSkill(x));
            _currentHP = _MaxHP;
        }

        public void SendDamage(float damage, BattleAttribute type)
        {
            if (IsGuard)
            {
                damage *= 0.75F;
            }

            _currentHP -= damage;
            IsGuard = false;
        }
    }   
}