using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAndAlive : MonoBehaviour
{
    int maxHp = 100;
    int Hp;
    bool isDie = false;
    [SerializeField]
    CapsuleCollider ccap;
    public bool active = false;

    void Awake()
    {
        ccap = GetComponent<CapsuleCollider>();
        Hp = maxHp;
    }
    private void OnEnable()
    {
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {

        WaitForSeconds deHp = new WaitForSeconds(1f);
        while (!isDie)
        {
            yield return deHp;
            Hp -= 30;
            if (Hp <= 0)
            {
                isDie = true;
                gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        isDie = false;
        Hp = maxHp;
    }
}
