using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            player = GetComponentInParent<Player>();
            Debug.Log(player.isJump);
            player.isJump = false;
        
        }
    }

}
