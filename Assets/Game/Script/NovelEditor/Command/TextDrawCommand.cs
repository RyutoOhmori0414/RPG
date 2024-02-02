using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

namespace NovelGameEditor
{
    [Serializable]
    public class TextDrawCommand : INovelCommand
    {
        [Header(nameof(TextDrawCommand))]
        [SerializeField, TextArea]
        private string _text = "";
        [SerializeField]
        private float _durateion = 1F;
        [SerializeField]
        private AudioClip _voice = default;
        [SerializeField]
        private bool _useVoiceTime = false;
        [SerializeField]
        private float _fadeSpeed = 1.0F;
        [SerializeField]
        private Color _textColor = Color.black;

        private TMP_TextInfo _info;
        private CommandData _commandData;
        private CancellationTokenSource _cancellationTS;
        private bool _isRunning;

        public TextDrawCommand()
        {
        }

        public TextDrawCommand(string text, float duration)
        {
            _text = text;
            _durateion = duration;
        }

        IEnumerator MessageUpdate()
        {
            yield return null;
            
            var currentIndex = -1;

            if (currentIndex + 1 >= _text.Length)
            {
                yield break;
            }

            // 初期化
            var elapsed = 0.0F;
            _cancellationTS = new();
            _isRunning = true;
            _info = _commandData.NovelTMP.textInfo;

            var interval = (_useVoiceTime ? _voice.length : _durateion) / (_text.Length + 1);

            _commandData.NovelTMP.text = _text;
            _commandData.NovelTMP.color = new Color(
                _commandData.NovelTMP.color.r,
                _commandData.NovelTMP.color.g,
                _commandData.NovelTMP.color.b,
                0.0F);

            if (_voice != null)
            {
                AudioSource.PlayClipAtPoint(_voice, Vector3.zero);
            }

            while (true)
            {
                if (_cancellationTS.IsCancellationRequested)
                {
                    _isRunning = false;
                    _commandData.NovelTMP.color = _textColor;
                    yield break;
                } // Cancel処理

                if (IsPrinting())
                {
                    elapsed += Time.deltaTime;
                    if (elapsed > interval)
                    {
                        currentIndex++;
                        elapsed = 0;
                        _commandData.MonoBehaviour.StartCoroutine(Change(currentIndex, _cancellationTS.Token));
                    }

                    yield return null;
                }
                else
                {
                    _commandData.NovelTMP.color = _textColor;
                    _isRunning = false;
                    yield break;
                } // 正規終了処理
            }

            bool IsPrinting()
            {
                return currentIndex + 1 < _text.Length;
            }
        }

        /// <summary>穏やかに色を変更する</summary>
        /// <param name="index">げんざいのTextのIndex</param>
        /// <returns></returns>
        IEnumerator Change(int index, CancellationToken token)
        {
            float elapsed = 0F;
            var temp = _textColor;
            
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    temp.a = 1F;
                    ChangeColor(index, temp);
                    _commandData.NovelTMP.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    yield break;
                }; // Cancel処理

                if (elapsed < _fadeSpeed)
                {
                    // 1Frame毎に緩やかに変化させる
                    temp.a = elapsed / _fadeSpeed;
                    elapsed += Time.deltaTime;
                    ChangeColor(index, temp);
                    _commandData.NovelTMP.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    yield return null;
                }
                else
                {
                    temp.a = 1F;
                    ChangeColor(index, temp);
                    _commandData.NovelTMP.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    yield break;
                } // 正規終了処理
            }
        }

        /// <summary>現在のTextの色を変更</summary>
        /// <param name="index">現在表示されている文字のIndex</param>
        /// <param name="col">変更したい色</param>
        private void ChangeColor(int index, Color col)
        {
            // 変更したいCharの情報を取得
            var characterInfo = _info.characterInfo[index];

            // 使用しているマテリアルと頂点のIndexを使用
            var mIndex = characterInfo.materialReferenceIndex;
            var vIndex = characterInfo.vertexIndex;

            // メッシュ毎のカラーの配列を取得
            var colors = _info.meshInfo[mIndex].colors32;

            // メッシュは4角形なのでその頂点色を変更する
            for (var i = 0; i < 4; i++)
            {
                colors[i + vIndex] = col;
            }
        }

        public IEnumerator RunCommandAsync() => MessageUpdate();

        public void SetCommandData(CommandData data) => _commandData = data;

        public void Skip()
        {
            _cancellationTS.Cancel();
        }

        public bool IsRunning => _isRunning;

        public void Dispose()
        {
            _cancellationTS.Cancel();
            _cancellationTS?.Dispose();
            _cancellationTS = new CancellationTokenSource();
        }
    }
}
