using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NovelGameEditor
{
    public interface INovelCommand : IDisposable
    {
        public IEnumerator RunCommandAsync();

        public void SetCommandData(CommandData data);

        public void Skip();

        public bool IsRunning { get; }
    }   
}
