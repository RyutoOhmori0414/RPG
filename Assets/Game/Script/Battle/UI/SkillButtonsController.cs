using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Battle.Player;
using RPG.Battle.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Battle.UI
{
    public class SkillButtonsController : MonoBehaviour
    {
        [SerializeField]
        private BattlePlayerController _playerController;

        [SerializeField]
        private Button[] _buttonTexts;

        private void OnEnable()
        {
            for (int i = 0; i < _buttonTexts.Length; i++)
            {
                if (_playerController.PlayerSkills.Length > i)
                {
                    var button = _buttonTexts[i];
                    var skill = _playerController.PlayerSkills[i];
                    var tempText = button.GetComponentInChildren<TMP_Text>();
                    
                    
                    tempText.text = skill.SkillName;
                }
                else
                {
                    _buttonTexts[i].gameObject.SetActive(false);
                }
            }
        }
    }   
}