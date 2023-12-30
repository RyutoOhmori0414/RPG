using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Adventure.Input
{
    public readonly struct PlayerAdventureInput : IEquatable<PlayerAdventureInput>
    {
        /// <summary>入力方向</summary>
        public Vector2 Move { get; }
        
        /// <summary>走りの入力がされているかどうか</summary>
        public bool IsRunInput { get; }
        
        /// <summary>決定ボタンが入力されているか</summary>
        public bool IsDecideInput { get; }

        public PlayerAdventureInput(Vector2 move, bool isRunInput, bool isDecideInput)
        {
            Move = move;
            IsRunInput = isRunInput;
            IsDecideInput = isDecideInput;
        }


        public bool Equals(PlayerAdventureInput other)
        {
            return Move.Equals(other.Move) && IsRunInput == other.IsRunInput && IsDecideInput == other.IsDecideInput;
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerAdventureInput other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Move, IsRunInput, IsDecideInput);
        }
    }    
}
