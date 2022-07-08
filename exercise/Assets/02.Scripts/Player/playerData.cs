using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerData : MonoBehaviour
{

    [HideInInspector] public int pindex;
    [HideInInspector] public int job;
    [HideInInspector] public int lv;
    [HideInInspector] public string name;
    [HideInInspector] public int hpMax;
    [HideInInspector] public int hpCur;

    [HideInInspector] public int mpMax;
    [HideInInspector] public int mpCur;
    [HideInInspector] public int str;
    [HideInInspector] public int dex;
    [HideInInspector] public int inter;
    [HideInInspector] public int def;

    [HideInInspector] public double damage;
    [HideInInspector] public int statP;
    [HideInInspector] public int skillP;
    [HideInInspector] public int gold;
    [HideInInspector] public int curExp;

    [HideInInspector] public int weaponItem;
    [HideInInspector] public int defendItem;
    [HideInInspector] public int accItem;
    [HideInInspector] public int scence;

    private float x;
    private float y;
    private float z;

    [HideInInspector] public Vector3 startPos;
    public Transform tr;

    public Animator anim;
    public Rigidbody rbody;
    public CapsuleCollider capcol;

    #region 개발 완료 이후에 사용할 데이터 자료 구조
    /* 
    public void setPlayerIndex(int _pindex)
    {
        pindex = _pindex;
        setData(pindex);
    }

    public void setData(int pindex)//몬스터에서 직접접근해야함(public)
    {
        job = 1;
        lv = 1;
        name = "aa";
        hp = 100;
        mp = 100;

        str = 1;
        dex = 1;
        inter = 1;
        def = 1;
        damage = 1;

        statP = 1;
        skillP = 1;
        gold = 1;
        curExp = 1;
        weaponItem = 1;

        defendItem = 1;
        accItem = 1;
        scence = 2;

        x = -192;
        y = 0;
        z = 140;

        startPos = new Vector3(x, y, z);
        tr = transform;

        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        capcol = GetComponent<CapsuleCollider>();
    }
    */
    #endregion

    public void testInit() 
        // test 초기화 용도
        // 1차 초기화는 playerLogin에서 처리 됨 
        // 기타 로직은 몬스터 로직과 동일
    {
        pindex = 1;
        job = 1;
        lv = 1;
        name = "aa";
        hpMax = 100;
        hpCur = 10;

        mpMax = 100;
        mpCur = 10;
        str = 2;
        dex = 2;
        inter = 2;
        def = 2;

        damage = 1;
        statP = 0;
        skillP = 0;
        gold = 1_000_000;
        curExp = 1;

        weaponItem = 1;
        defendItem = 1;
        accItem = 1;
        scence = 2;

        x = -140;
        y = 0.3f;
        z = 140;

        startPos = new Vector3(x, y, z);
        tr = transform;

        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        capcol = GetComponent<CapsuleCollider>();
    }
}
