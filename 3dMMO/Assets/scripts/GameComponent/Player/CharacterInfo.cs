using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInfo
{
    public struct ChaInfo
    {
        public int _hp;
        public int _coin;
        public int _attack;
        public int _defence;
    };

    public class CharacterManager
    {
        private static CharacterManager instance;
        public ChaInfo myCharacter;

        // private 생성자: 외부에서 인스턴스 생성 불가능
        private CharacterManager() { }

        // public static 메서드: 단일 인스턴스 반환
        public static CharacterManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CharacterManager();
                }
                return instance;
            }
        }
    }
}
