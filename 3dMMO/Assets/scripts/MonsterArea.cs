using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArea : MonoBehaviour
{
    // Start is called before the first frame update
    Collider colid;
    public bool attackStart=false;
    void Start()
    {
        colid = gameObject.GetComponent<Collider>();
    }
    void Update()
    {
        if (attackStart)
        {
            attackStart = false;
            Debug.Log("공격켜짐");
            colid.enabled = true;
            StartCoroutine("Attack");
        }
    }
    IEnumerable Attack()
    {
        yield return new WaitForSeconds(1f);
        colid.enabled = false;
        yield return new WaitForSeconds(1f);

    }
    public void AttackStart()
    {
        Debug.Log("공격!");
       // StartCoroutine("Attack");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("monster     " + other.gameObject.name);
            //StartCoroutine("Attack");
        }
    }
}
