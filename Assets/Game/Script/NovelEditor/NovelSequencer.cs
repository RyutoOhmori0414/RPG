using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NovelGameEditor
{
    public class NovelSequencer : MonoBehaviour
    {
        [SerializeField]
        private NovelDataContainerScriptableObject _contaner = default;

        [SerializeField] private TMP_Text _printer = default;
        [SerializeField] private Image[] _charaImages;
        [SerializeField] private Image[] _charaDiff;
        [SerializeField] private Image _backGround = default;
        [SerializeField] private Image _backGroundDiff = default;

        private CancellationTokenSource _cancellationTokenSource;
        private INovelCommand _current;
        
        private void Awake()
        {
            var commandData = new CommandData(_printer, _charaImages, _charaDiff, _backGround, _backGroundDiff, this);

            for (int i = 0; i < _contaner.NovelData.Count; i++)
            {
                _contaner.NovelData[i].SetCommandData(commandData);
            }
        }

        private void Start()
        {
            StartCoroutine(PlayNovel(_contaner.NovelData));
        }

        private IEnumerator WaitSkip(INovelCommand command)
        {
            StartCoroutine(command.RunCommandAsync());
            
            while (true)
            {
                yield return null;
                
                if (!command.IsRunning)
                {
                    break;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    command.Skip();
                    break;
                }
            }
        }

        private IEnumerator PlayNovel(List<INovelCommand> novelData)
        {
            foreach (var i in novelData)
            {
                yield return WaitSkip(i);

                yield return null;
                yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
            }
        }
    }
}
