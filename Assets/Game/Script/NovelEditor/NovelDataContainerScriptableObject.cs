using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NovelGameEditor
{
    [CreateAssetMenu(fileName = "DataContainer", menuName = "ScriptableObjects/NovelDataContainer", order = 1)]
    public class NovelDataContainerScriptableObject : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        private List<INovelCommand> _novelData;

        public List<INovelCommand> NovelData => _novelData;

        private void OnDestroy()
        {
            foreach (var i in _novelData)
            {
                i.Dispose();
            }
        }
    }
}
