using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NovelGameEditor
{
    [Serializable]
    public class ConnectCommand : INovelCommand
    {
        [SerializeReference, SubclassSelector]
        private INovelCommand[] _commands;

        private bool _isRunning = false;
        private CancellationTokenSource _cancellationTS;
        private INovelCommand _current;

        private IEnumerator Connect()
        {
            _isRunning = true;
            _cancellationTS = new();
            
            foreach (var i in _commands)
            {
                _current = i;
                yield return i.RunCommandAsync();

                if (_cancellationTS.IsCancellationRequested)
                {
                    break;
                }
            }

            _isRunning = false;
        }
        
        public void Dispose()
        {
            _cancellationTS.Cancel();
            _cancellationTS.Dispose();
        }

        public IEnumerator RunCommandAsync() => Connect();

        public void SetCommandData(CommandData data)
        {
            foreach (var i in _commands)
            {
                i.SetCommandData(data);
            }
        }

        public void Skip()
        {
            _cancellationTS.Cancel();
            _current.Skip();

            _isRunning = false;
        }

        public bool IsRunning => _isRunning;
    }
}


