using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NovelGameEditor
{
    [Serializable]
    public class SimpleFadeCommand : INovelCommand
    {
        [SerializeField]
        private FadeTarget _fadeTarget = FadeTarget.Left;

        [SerializeField]
        private bool _fadeAlphaOnly = false;
        
        [SerializeField]
        private Color _fadeColor = Color.clear;

        [SerializeField]
        private float _duration = 1F;

        private CommandData _commandData = default;
        private bool _isRunning = false;
        private CancellationTokenSource _cancellationTS = default;

        private IEnumerator PlayFade()
        {
            _isRunning = true;
            var elapsed = 0.0F;
            var targetImage = _commandData.CharaImages[(int)_fadeTarget];
            var firstColor = targetImage.color;
            _cancellationTS = new();

            while (true)
            {
                if (_cancellationTS.IsCancellationRequested)
                {
                    Fade(1.0F);
                    yield break;
                }
                
                if (elapsed < _duration)
                {
                    Fade(elapsed / _duration);
                }
                else
                {
                    Fade(1.0F);
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
            
            void Fade(float current)
            {
                if (_fadeAlphaOnly)
                {
                    var temp = targetImage.color;
                    temp.a = Mathf.Lerp(firstColor.a, _fadeColor.a, current);
                    targetImage.color = temp;
                }
                else
                {
                    targetImage.color = Color.Lerp(firstColor, _fadeColor, current);
                }
            }
        }
        

        public void Dispose()
        {
            _cancellationTS.Cancel();
            _cancellationTS.Dispose();
        }

        public IEnumerator RunCommandAsync() => PlayFade();

        public void SetCommandData(CommandData data)
        {
            _commandData = data;
        }

        public void Skip()
        {
            _cancellationTS.Cancel();
        }

        public bool IsRunning => _isRunning;

        [Serializable]
        public enum FadeTarget
        {
            Left,
            Center,
            Right
        }
    }   
}
