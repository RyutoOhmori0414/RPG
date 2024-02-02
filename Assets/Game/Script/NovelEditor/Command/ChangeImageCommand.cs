using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NovelGameEditor
{
    [Serializable]
    public class ChangeImageCommand : INovelCommand
    {
        [SerializeField] private Target _target = Target.BackGround;
        [SerializeField] private EffectType _effectType = EffectType.Dissolve;
        [SerializeField] private float _duration = 2F;
        [SerializeField] private Sprite _changeSprite = default;

        private bool _isRunning = false;
        private CommandData _commandData;
        private CancellationTokenSource _cancellationTokenSource;

        private IEnumerator ChangeImage()
        {
            float elapsed = 0.0F;
            _isRunning = true;
            _cancellationTokenSource = new();

            Image target = default;
            Image targetDiff = default;
            if (_target == Target.BackGround)
            {
                target = _commandData.BackgroundImage;
                targetDiff = _commandData.BackgronndDiff;
            }
            else
            {
                target = _commandData.CharaImages[(int)_target];
                targetDiff = _commandData.Diff[(int)_target];
            }

            targetDiff.sprite = _changeSprite;
            targetDiff.SetNativeSize();
            targetDiff.enabled = true;

            if (_effectType == EffectType.Dissolve)
            {
                target.material.DisableKeyword("_EFFECT_NONE");
                target.material.EnableKeyword("_EFFECT_DISSOLVE");
                targetDiff.material.DisableKeyword("_EFFECT_NONE");
                targetDiff.material.EnableKeyword("_EFFECT_DISSOLVE");
            }

            while (true)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    ImageChange(0);
                    target.sprite = _changeSprite;
                    target.SetNativeSize();
                    
                    targetDiff.enabled = false;
                    target.material.DisableKeyword("_EFFECT_DISSOLVE");
                    target.material.EnableKeyword("_EFFECT_NONE");
                    targetDiff.material.DisableKeyword("_EFFECT_DISSOLVE");
                    targetDiff.material.EnableKeyword("_EFFECT_NONE");
                    targetDiff.material.SetFloat("_Amount", 1);
                    yield break;
                }
                
                if (elapsed < _duration)
                {
                    ImageChange(elapsed / _duration);
                }
                else
                {
                    ImageChange(0);
                    target.sprite = _changeSprite;
                    target.SetNativeSize();
                    targetDiff.enabled = false;
                    target.material.DisableKeyword("_EFFECT_DISSOLVE");
                    target.material.EnableKeyword("_EFFECT_NONE");
                    targetDiff.material.DisableKeyword("_EFFECT_DISSOLVE");
                    targetDiff.material.EnableKeyword("_EFFECT_NONE");
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
            
            void ImageChange(float current)
            {
                if (_effectType == EffectType.Dissolve)
                {
                    target.material.SetFloat("_Amount", current);
                }
                else
                {
                    var targetCol = target.color;
                    targetCol.a = 1 - current;
                    target.color = targetCol;

                    var targetDiffCol = targetDiff.color;
                    targetDiffCol.a = current;
                    targetDiff.color = targetDiffCol;
                }
            }
        }
        
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public IEnumerator RunCommandAsync() => ChangeImage();

        public void SetCommandData(CommandData data) => _commandData = data;

        public void Skip()
        {
            _cancellationTokenSource.Cancel();
        }

        public bool IsRunning => _isRunning;

        [Serializable]
        public enum Target
        {
            CharaLeft,
            CharaCenter,
            CharaRight,
            BackGround
        }

        [Serializable]
        public enum EffectType
        {
            Fade,
            Dissolve
        }
    }   
}