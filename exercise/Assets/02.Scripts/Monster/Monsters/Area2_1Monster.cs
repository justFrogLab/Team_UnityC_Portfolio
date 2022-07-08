using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Area2_1Monster : Monster
{
    void Start()
    {
        SetMainIndex(2050);
        tr = transform;
        IsDie = false;
        currentHp = Hp;
        StartMonster = true;
        Invoke("Getdam", 10f);
    }
    private void OnDisable()
    {
        currentHp = Hp;
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
    }
    void Getdam()
    {
        currentHp = 0;
        StartCoroutine(GetDamage());
    }
    new IEnumerator GetDamage()
    {
        if (currentHp <= 0)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<MeshCollider>().enabled = false;
            StartCoroutine(DropItem());
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
        yield return null;
    }
}
