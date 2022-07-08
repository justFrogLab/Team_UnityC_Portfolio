using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Area1_1Monster : Monster
{

    void Start()
    {
        SetMainIndex(2010);
        tr = transform;
        IsDie = false;
        currentHp = Hp;
        _Ani = GetComponent<Animator>();
        StartMonster = true;
        Invoke("Getdam", 3f);
    }
    private void OnDisable()
    {
        currentHp = Hp;
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
    }
    void Getdam()
    {
        currentHp = 0;
        StartCoroutine(GetDamage());
    }
    new IEnumerator GetDamage()
    {
        _Ani.SetTrigger("IsHit");
        if (currentHp <= 0)
        {
            _Ani.SetTrigger("IsDie");
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(DropItem());
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
        yield return null;
    }
}
