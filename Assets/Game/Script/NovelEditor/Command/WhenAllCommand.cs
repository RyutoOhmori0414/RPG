using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace NovelGameEditor
{
    [Serializable]
    public class WaitAllCommand : INovelCommand
    {
        [SerializeReference, SubclassSelector]
        private INovelCommand[] _commands = default;
    
        private CommandData _commandData = default;
        private List<bool> _runFlags = new ();
        private bool _isRunning = false;
        
        public WaitAllCommand() { }
        public WaitAllCommand(params INovelCommand[] waitCommands) => _commands = waitCommands;

        private IEnumerator WaitCommands()
        {
            _isRunning = true;
            
            foreach (var i in _commands)
            {
                _commandData.MonoBehaviour.StartCoroutine(RunCommand(i.RunCommandAsync()));
            }

            yield return new WaitUntil(() => _runFlags.All(y => y));
            _isRunning = false;
        }

        private IEnumerator RunCommand(IEnumerator command)
        {
            _runFlags.Add(false);
            var index = _runFlags.Count - 1;
            
            yield return command;
            _runFlags[index] = true;
        }
        
        public IEnumerator RunCommandAsync() => WaitCommands();

        public void SetCommandData(CommandData data)
        {
            _commandData = data;

            foreach (var i in _commands)
            {
                i.SetCommandData(data);
            }
        }

        public void Skip()
        {
            foreach (var i in _commands)
            {
                i.Skip();
            }
            
            _isRunning = false;
        }

        public bool IsRunning => _isRunning;

        public void Dispose()
        {
        }
    }   
}
