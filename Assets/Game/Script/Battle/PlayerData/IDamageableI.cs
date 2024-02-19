using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Battle.Player
{
    public interface IDamage
    {
        public void SendDamage(float damage, BattleAttribute type);
    }   
}