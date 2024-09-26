using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInfo
{

    public struct ChaInfo
    {
        public int _hp;
        public int _mp;
        public int _money;
        public int _level;
    };
    public struct SkillInfo
    {
        public int _attack1;
        public int _attack2;
        public int _attack3;
        public int _attack4;
    };

    public class CharacterManager
    {
        private static CharacterManager instance;
        public ChaInfo myCharacter;
        public string _username;
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
        public void InitializePlayer(ChaInfo playerInfo, string username)
        {
            myCharacter._hp = playerInfo._hp;
            myCharacter._mp = playerInfo._mp;
            myCharacter._money = playerInfo._money;
            myCharacter._level = playerInfo._level;
            _username = username;
        }
    }


}
