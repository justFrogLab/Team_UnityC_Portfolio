using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Area2_2Monster : MonsterSpecial
{
    void Start()
    {
        SetMainIndex(2012);
        tr = transform;
        agent = GetComponent<NavMeshAgent>();
        _Ani = GetComponent<Animator>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.enabled = false;
        detectdist = 5f;
        attackdist = 2f;
        rangedist = 4f;
        IsDie = false;
        movedist = 2f;
        attackangle = 120f;
        rangeangle = 15f;
        currentHp = Hp;
        RangeAndNear = true;
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
