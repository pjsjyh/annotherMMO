using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gamePanel;
    public Player player;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerMpText;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI playerCoinText;
    public RectTransform playerHealthBar;
    public RectTransform playerMpBar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        playerHealthText.text = player.playerInfo._hp+" / "+"100";
        if (player!=null)
        {
            if((float)player.playerInfo._hp / 100>=0)
                playerHealthBar.localScale = new Vector3((float)player.playerInfo._hp / 100, 1, 1);
        }
        
    }

}
