using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Battle.Player
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData", order = 1)]
    public class PlayerSkillData : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        private IPlayerSkill[] _playerSkills;

        public IPlayerSkill GetSkill(string skillName)
        {
            return Array.Find(_playerSkills, x => x.SkillName == skillName);
        }
    }   
}