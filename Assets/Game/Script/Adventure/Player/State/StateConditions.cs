using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGAdventure
{
    public class StateConditions : ICollection<Func<Type>>
    {
        private List<Func<Type>> _conditions = default;

        public StateConditions(params Func<Type>[] list) => _conditions = list.ToList();

        public IEnumerator<Func<Type>> GetEnumerator() => new Enumerator(_conditions);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Func<Type> item) => Debug.LogWarning($"{nameof(StateConditions)}は読み取り専用です");

        public void Clear() => Debug.LogWarning($"{nameof(StateConditions)}は読み取り専用です");

        public bool Contains(Func<Type> item) => _conditions.Contains(item);

        public void CopyTo(Func<Type>[] array, int arrayIndex) => _conditions.CopyTo(array, arrayIndex);

        public bool Remove(Func<Type> item)
        {
            Debug.LogWarning($"{nameof(StateConditions)}は読み取り専用です");
            return false;
        }

        public int Count => _conditions.Count;
        public bool IsReadOnly => true;

        /// <summary>このコレクションを調べて遷移先を返す</summary>
        /// <returns>遷移先 or null(遷移しない)</returns>
        public Type Check()
        {
            foreach (var i in this)
            {
                var temp = i.Invoke();

                if (temp is not null)
                {
                    return temp;
                }
            }

            return null;
        }
        
        public struct Enumerator : IEnumerator<Func<Type>>
        {
            /// <summary>対象のList</summary>
            private List<Func<Type>> _list;
            /// <summary>現在のIndex</summary>
            private int _cursor;

            public Enumerator(List<Func<Type>> list)
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

            public Func<Type> Current
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
