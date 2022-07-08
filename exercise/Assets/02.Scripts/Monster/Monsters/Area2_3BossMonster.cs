using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Area2_3BossMonster : MonsterBoss
{
    void Start()
    {
        SetMainIndex(2090);
        tr = transform;
        agent = GetComponent<NavMeshAgent>();
        _Ani = GetComponent<Animator>();
        //agent.autoBraking = false;
        agent.updateRotation = false;
        agent.enabled = false;
        detectdist = 15f;
        attackdist = 8f;
        attackangle = 20;
        Cannondist = 4f;
        Missiledist = 6f;
        IsDie = false;
        currentHp = Hp;
        StartMonster = true;
        StartCoroutine(DetectPlayer());
    }
    private void OnDisable()
    {
        currentHp = Hp;
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        if (StartMonster)
        {
            GetComponent<ActiveData>().Detectactive = true;
            GetComponent<ActiveData>().Traceactive = false;
            GetComponent<ActiveData>().Attackactive = false;
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            StartCoroutine(DetectPlayer());
        }
    }
}
