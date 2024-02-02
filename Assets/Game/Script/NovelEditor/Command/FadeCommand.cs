using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NovelGameEditor
{
    [Serializable]
    public class FadeCommand : INovelCommand
    {
        [SerializeField]
        private FadeTarget _fadeTarget = FadeTarget.Left;
        [SerializeField]
        private FadeType _effectType = FadeType.Dissolve; 
        [SerializeField]
        private FadeMode _fadeMode = FadeMode.Out;
        
        [SerializeField]
        private float _duration = 1F;

        private CommandData _commandData = default;
        private bool _isRunning = false;
        private CancellationTokenSource _cancellationTS = default;
        private int _amountID = Shader.PropertyToID("_Amount");

        private IEnumerator PlayDissolve()
        {
            _isRunning = true;
            var elapsed = 0.0F;
            var targetImage = _commandData.CharaImages[(int)_fadeTarget];
            var firstColor = targetImage.color;
            _cancellationTS = new();
            
            firstColor.a = 1F;
            targetImage.color = firstColor;
            
            targetImage.material.SetFloat(_amountID, _fadeMode == FadeMode.In ? 1F : 0F);
            targetImage.material.DisableKeyword("_EFFECT_NONE");
            targetImage.material.EnableKeyword("_EFFECT_DISSOLVE");

            while (true)
            {
                if (_cancellationTS.IsCancellationRequested)
                {
                    Dissolve(_fadeMode == FadeMode.In ? 0F : 1F);
                    firstColor.a = _fadeMode == FadeMode.Out ? 0F : 1F;
                    targetImage.color = firstColor;
                    targetImage.material.DisableKeyword("_EFFECT_DISSOLVE");
                    targetImage.material.EnableKeyword("_EFFECT_NONE");
                    yield break;
                }
                
                if (elapsed < _duration)
                {
                    Dissolve(elapsed / _duration);
                }
                else
                {
                    Dissolve(_fadeMode == FadeMode.In ? 0F : 1F);
                    firstColor.a = _fadeMode == FadeMode.Out ? 0F : 1F;
                    targetImage.color = firstColor;
                    targetImage.material.DisableKeyword("_EFFECT_DISSOLVE");
                    targetImage.material.EnableKeyword("_EFFECT_NONE");
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
            
            void Dissolve(float current)
            {
                if (_fadeMode == FadeMode.Out)
                {
                    targetImage.material.SetFloat(_amountID, current);
                }
                else
                {
                    
                    targetImage.material.SetFloat(_amountID, 1 - current);
                }
            }
        }

        private IEnumerator PlayFade()
        {
            _isRunning = true;
            var elapsed = 0.0F;
            var targetImage = _commandData.CharaImages[(int)_fadeTarget];
            var firstColor = targetImage.color;
            var firstAlpha = targetImage.color.a;
            var targetAlpha = _fadeMode == FadeMode.In ? 1F : 0F;
            _cancellationTS = new();

            while (true)
            {
                if (_cancellationTS.IsCancellationRequested)
                {
                    firstColor.a = targetAlpha;
                    targetImage.color = firstColor;
                    yield break;
                }

                if (elapsed < _duration)
                {
                    firstColor.a = Mathf.Lerp(firstAlpha, targetAlpha, elapsed / _duration);
                    targetImage.color = firstColor;
                }
                else
                {
                    firstColor.a = targetAlpha;
                    targetImage.color = firstColor;
                    yield break;
                }
                
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        

        public void Dispose()
        {
            _cancellationTS.Cancel();
            _cancellationTS.Dispose();
        }

        public IEnumerator RunCommandAsync() => _effectType == FadeType.Dissolve ? PlayDissolve() : PlayFade();

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
        [Serializable]
        public enum FadeType
        {
            None,
            Dissolve
        }
        
        [Serializable]
        public enum FadeMode
        {
            In,
            Out
        }
    }   
}
