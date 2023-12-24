using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private SortedDictionary<Type, AbstractState> _cache = new ();

        private void Awake()
        {
            _cache.Add(typeof(PlayerWalkState), new PlayerWalkState());
            
            
        }
    }
}
