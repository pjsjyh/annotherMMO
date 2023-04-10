using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : CharacterInfo
{
    // Start is called before the first frame update
    public enum Type
    {
        slime, blueSlime
    };
    public Type monType;
    public ChaInfo monInfo;

    bool ischasePlayer = false;
    Transform toChase;
    Animator monsterAnim;

    public NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        monsterAnim = GetComponent<Animator>();
        createMon();
    }

    // Update is called once per frame
    void Update()
    {
        if (ischasePlayer)
        {
            chase();
            monsterAnim.SetBool("isFollow", true);
            if (nav.remainingDistance > 10)
            {
                ischasePlayer = false;
                monsterAnim.SetBool("isFollow", false);
            }
        }
    }

    void createMon()
    {
        switch(monType)
        {
            case Type.slime:
                monInfo._hp = 100;
                monInfo._attack = 30;
                monInfo._defence = 10;
                break;
            case Type.blueSlime:
                monInfo._hp = 150;
                monInfo._attack = 50;
                monInfo._defence = 30;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        ischasePlayer = true;
        toChase = other.gameObject.transform;
    }
   
    void chase()
    {
        nav.SetDestination(toChase.position);
    }
}
