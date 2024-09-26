﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharacterInfo;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }


    public GameObject gamePanel;
    public Player player;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerMpText;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI playerCoinText;
    public TextMeshProUGUI playerNameText;
    public RectTransform playerHealthBar;
    public RectTransform playerMpBar;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
                Destroy(this.gameObject);
        }
    }

    void LateUpdate()
    {
        playerHealthText.text = CharacterManager.Instance.myCharacter._hp + " / " + "100";
        playerLevelText.text = CharacterManager.Instance.myCharacter._level.ToString();
        playerNameText.text = CharacterManager.Instance._username;
        playerCoinText.text = string.Format("{0:n0}", CharacterManager.Instance.myCharacter._money);
        if (player != null)
        {
            if ((float)CharacterManager.Instance.myCharacter._hp / 100 >= 0)
                playerHealthBar.localScale = new Vector3((float)CharacterManager.Instance.myCharacter._hp / 100, 1, 1);
        }

    }

}
