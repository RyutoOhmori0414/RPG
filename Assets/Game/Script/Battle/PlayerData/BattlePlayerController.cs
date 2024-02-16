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
    public class BattlePlayerController : MonoBehaviour
    {
        [SerializeField]
        private string[] _loadSkillNames = default;
        
        [SerializeField]
        private PlayerSkillData _skills = default;

        [Inject]
        private ISubscriber<PhaseParams> _subscriber;
        
        private IPlayerSkill[] _playerSkills;
        public List<IPlayerSkill> PlayerSkills => _playerSkills.ToList();

        private void Awake()
        {
            _playerSkills = Array.ConvertAll(_loadSkillNames, x => _skills.GetSkill(x));
        }
    }   
}