using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Area2_2ChampMonster : MonsterSpecial
{
    void Start()
    {
        SetMainIndex(2080);
        tr = transform;
        agent = GetComponent<NavMeshAgent>();
        _Ani = GetComponent<Animator>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.enabled = false;
        detectdist = 5f;
        attackdist = 2f;
        Circledist = 4f;
        IsDie = false;
        movedist = 2f;
        attackangle = 120f;
        attackPatton = 3;
        currentHp = Hp;
        RangeAndNear = false;
        StartMonster = true;
        StartCoroutine(Patrol());
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
            GetComponent<ActiveData>().ActiveEndPosition = false;
            GetComponent<ActiveData>().Traceactive = false;
            GetComponent<ActiveData>().Patrolactive = true;
            GetComponent<ActiveData>().Collisionactive = false;
            GetComponent<ActiveData>().Attackactive = false;
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            StartCoroutine(Patrol());
        }
    }
}
