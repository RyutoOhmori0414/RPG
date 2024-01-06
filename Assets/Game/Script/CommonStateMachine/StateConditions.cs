using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.CommonStateMachine
{
    public class StateConditions : ICollection<Func<bool>>
    {
        private List<Func<bool>> _conditions = default;

        public StateConditions(params Func<bool>[] list) => _conditions = list.ToList();

        public IEnumerator<Func<bool>> GetEnumerator() => new Enumerator(_conditions);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Func<bool> item) => Debug.LogWarning($"{nameof(StateConditions)}は読み取り専用です");

        public void Clear() => Debug.LogWarning($"{nameof(StateConditions)}は読み取り専用です");

        public bool Contains(Func<bool> item) => _conditions.Contains(item);

        public void CopyTo(Func<bool>[] array, int arrayIndex) => _conditions.CopyTo(array, arrayIndex);

        public bool Remove(Func<bool> item)
        {
            Debug.LogWarning($"{nameof(StateConditions)}は読み取り専用です");
            return false;
        }

        public int Count => _conditions.Count;
        public bool IsReadOnly => true;

        /// <summary>このコレクションを調べて遷移したかを返す</summary>
        /// <returns>遷移したかどうか</returns>
        public bool Check()
        {
            if (_conditions is null) return false;
            
            foreach (var i in _conditions)
            {
                if (i is null) continue;
                
                var temp = i.Invoke();

                if (temp)
                {
                    return false;
                }
            }

            return false;
        }
        
        public struct Enumerator : IEnumerator<Func<bool>>
        {
            /// <summary>対象のList</summary>
            private List<Func<bool>> _list;
            /// <summary>現在のIndex</summary>
            private int _cursor;

            public Enumerator(List<Func<bool>> list)
            {
                _list = list;
                _cursor = -1;
            }
            
            public bool MoveNext()
            {
                if (_cursor < _list.Count)
                {
                    _cursor++;
                }

                return _cursor <= _list.Count;
            }

            public void Reset()
            {
                _cursor = -1;
            }

            object IEnumerator.Current => Current;

            public Func<bool> Current
            {
                get
                {
                    if (_cursor < 0 || _cursor >= _list.Count)
                    {
                        throw new InvalidOperationException();
                    }

                    return _list[_cursor];
                }
            }

            public void Dispose() { }
        }
    }   
}
