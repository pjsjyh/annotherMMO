﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public struct ChaInfo
    {
        public int _hp;
        public enum _chaType {Player, Monster };
        public int _attack;
        public int _defence;
    };
    
}
