using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterBaseMethod : MonoBehaviour
{
    public int mainIndex;
    public string monsterName;
    public int Hp;
    public int Lv;
    public int Damage;
    public int Defend;
    public int Type;
    public int Exp;
    public string[] dropItem;
    public string[] dropRate;
    public int Crit;
    public int Acc;
    public int CC;
    public int Avoid;
    public int DropMin;
    public int DropMax;
    public int EXPforSkill;
    public bool IsDie;

    public Transform tr;
    public NavMeshAgent agent;
    public Animator _Ani;
    public GameObject dropprefab;
    public float detectdist;
    public float movedist;
    public float attackdist;
    public float attackangle;
    public float currentHp;
    public bool StartMonster = false;
    public void SetMainIndex(int targetIndex)//몬스터에서 직접접근해야함(public)
    {
        var monsterInfo = csvReader.monsterData[csvReader.monsterIndexPairs[targetIndex]];
        mainIndex = (int)monsterInfo[0];
        monsterName = (string)monsterInfo[1];
        Hp = (int)monsterInfo[2];
        Lv = (int)monsterInfo[3];
        Damage = (int)monsterInfo[4];
        Defend = (int)monsterInfo[5];
        Type = (int)monsterInfo[6];
        Exp = (int)monsterInfo[7];
        dropItem = monsterInfo[8].ToString().Split('@');
        dropRate = monsterInfo[9].ToString().Split('@');
    }
    public virtual IEnumerator GetDamage()
    {
        _Ani.SetTrigger("IsHit");
        if (currentHp <= 0)
        {
            _Ani.SetTrigger("IsDie");
            agent.enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(DropItem());
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
        yield return null;
    }
    public IEnumerator DropItem()
    {
        yield return null;
        List<string> drop = new List<string>();
        string objname = null;
        for (int i = 0; i < dropItem.Length; i++)
        {
            if (Random.Range(0, 100) <= int.Parse(dropRate[i]))
            {
                drop.Add(dropItem[i]);
            }
        }
        if (drop != null)
        {
            for (int i = 0; i < drop.ToArray().Length; i++)
            {
                if (i != 0)
                {
                    objname += '_';
                }
                objname += drop.ToArray()[i];
                if (i == drop.Count - 1)
                {
                    objname += "item";
                }
            }
            GameObject dropbox = PoolManager.poolmanager.GetDropBox();
            dropbox.name = objname;
            dropbox.SetActive(true);
            dropbox.transform.position = gameObject.transform.position;
        }
    }
}
