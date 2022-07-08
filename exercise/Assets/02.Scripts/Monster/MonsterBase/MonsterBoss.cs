using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoss : MonsterBaseMethod
{
    public float Cannondist;
    public float Missiledist;
    public int attackPatton;
    [SerializeField]
    Transform[] MissilePos;
    public IEnumerator DetectPlayer()//몬스터에서 직접접근해야함(public)
    {//몬스터주변플레이어 감지함수
        Collider[] objs;//감지된 객체를 담기위한 콜라이더배열
        RaycastHit hit;//Ray를 맞은 대상
        Vector3 origin;//Ray를 쏘는 위치
        while (!IsDie)
        {
            while (gameObject.GetComponent<ActiveData>().Detectactive == true)
            {
                yield return null;
                if (IsDie) yield break;
                if (gameObject.GetComponent<ActiveData>().Detectactive == false) break;
                objs = Physics.OverlapSphere(transform.position, detectdist);
                foreach (Collider obj in objs)
                {
                    if (obj.tag == "Player")
                    {
                        origin = transform.position + (transform.up * 0.5f);
                        if (Physics.Raycast(origin, obj.gameObject.transform.position - transform.position, out hit, detectdist))
                        {
                            Debug.DrawRay(origin, (obj.gameObject.transform.position - transform.position), Color.green);
                            if (hit.collider.CompareTag("Player"))
                            {
                                StartCoroutine(Trace(obj.gameObject));
                                break;//플레이어가 주변에 여러명이있어도 처음 발견한 플레이어만 감지 후 foreach종료
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator Trace(GameObject Person)//피격 이후 / 탐지 이후 동작으로 직접접근할수없는 함수(private)
    {//추적하는 함수                (초기위치        ,타겟 위치)
        RaycastHit hit;//Ray를 맞은대상
        Vector3 origin;//Ray를 쏘는 위치
        agent.enabled = true;
        agent.isStopped = false;
        int attpatton;
        gameObject.GetComponent<ActiveData>().Detectactive = false;//코루틴을 중복 실행시키지 않기위한 bool변경
        gameObject.GetComponent<ActiveData>().Traceactive = true;//코루틴을 중복 실행시키지 않기위한 bool변경
        while (!IsDie && gameObject.GetComponent<ActiveData>().Traceactive)
        {
            if (IsDie) yield break;
            if (!gameObject.GetComponent<ActiveData>().Traceactive) yield break;
            yield return null;
            if (gameObject.GetComponent<ActiveData>().Attackactive) continue;
            _Ani.SetBool("IsTrace", true);
            origin = transform.position + (transform.up * 1f);
            agent.destination = Person.transform.position;
            transform.LookAt(Person.transform.position);
            if (Vector3.Distance(transform.position, Person.transform.position) < attackdist)
            {
                agent.velocity = Vector3.zero;
                _Ani.SetBool("IsTrace", false);
                attpatton = Random.Range(0, 10);
                if (currentHp < 30 && GetComponent<ActiveData>().is30pat == false && GetComponent<ActiveData>().is50pat == true && GetComponent<ActiveData>().is70pat == true)
                {
                    GetComponent<ActiveData>().is30pat = true;
                    StartCoroutine(DoHpAttack(Person));
                }
                else if (currentHp < 50 && GetComponent<ActiveData>().is50pat == false && GetComponent<ActiveData>().is70pat == true)
                {
                    GetComponent<ActiveData>().is50pat = true;
                    StartCoroutine(DoHpAttack(Person));
                }
                else if (currentHp < 70 && GetComponent<ActiveData>().is70pat == false)
                {
                    GetComponent<ActiveData>().is70pat = true;
                    StartCoroutine(DoHpAttack(Person));
                }
                else if (attpatton < 5)
                {
                    StartCoroutine(DoAttack(Person));
                }
                else
                {
                    StartCoroutine(DoSpecialAttack(Person));
                }
            }
        }
    }

    IEnumerator DoAttack(GameObject Person)
    {
        Vector3 Personorigin = Person.transform.position;
        gameObject.GetComponent<ActiveData>().Attackactive = true;
        transform.LookAt(Personorigin);
        agent.isStopped = true;
        yield return new WaitForSeconds(0.5f);
        _Ani.SetTrigger("IsAttack0");
        Vector3 dir1 = (Person.transform.position - transform.position).normalized;
        Vector3 dir2 = new Vector3(dir1.x, 0, dir1.z);//점프했을때 y축을 각도계산하지않기위해 추가
        AttackRange();//공격범위 표시
        Collider[] objs = Physics.OverlapSphere(transform.position, attackdist);
        foreach (Collider obj in objs)
        {
            if (obj.tag == "Player")
            {
                if (Vector3.Angle(transform.forward, dir2) < attackangle * 0.5f)
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                    yield return new WaitForSeconds(0.03f);
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
        }
        yield return new WaitForSeconds(0.22f);
        AttackRange();//공격범위 표시
        objs = Physics.OverlapSphere(transform.position, attackdist);
        foreach (Collider obj in objs)
        {
            if (obj.tag == "Player")
            {
                if (Vector3.Angle(transform.forward, dir2) < attackangle * 0.5f)
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                    yield return new WaitForSeconds(0.03f);
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
        }
        yield return new WaitForSeconds(0.22f);
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
        gameObject.GetComponent<ActiveData>().Attackactive = false;
    }
    IEnumerator DoSpecialAttack(GameObject Person)
    {
        Vector3 Personorigin = Person.transform.position;
        gameObject.GetComponent<ActiveData>().Attackactive = true;
        transform.LookAt(Personorigin);
        agent.isStopped = true;
        int attpa = Random.Range(0,3);

        if (attpa == 0)
        {
            ShowAttackRange(Personorigin, Cannondist);
            yield return new WaitForSeconds(1f);
            _Ani.SetTrigger("IsCannon0");
            Collider[] objs = Physics.OverlapSphere(Personorigin, Cannondist*0.5f);
            foreach (Collider obj in objs)
            {
                if (obj.tag == "Player")
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                    yield return new WaitForSeconds(0.03f);
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
            yield return new WaitForSeconds(0.47f);
            objs = Physics.OverlapSphere(Personorigin, Cannondist * 0.5f);
            foreach (Collider obj in objs)
            {
                if (obj.tag == "Player")
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                    yield return new WaitForSeconds(0.03f);
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
            yield return new WaitForSeconds(0.47f);
            yield return new WaitForSeconds(1f);
            agent.isStopped = false;
            gameObject.GetComponent<ActiveData>().Attackactive = false;
        }
        else if (attpa == 1)
        {
            yield return new WaitForSeconds(1f);
            _Ani.SetTrigger("IsMissile0");
            Vector3 CurPerson = Person.transform.position;
            ShowAttackRange(CurPerson, Missiledist);

            GameObject mis = PoolManager.poolmanager.GetMissile();
            mis.transform.position = MissilePos[0].position;
            mis.SetActive(true);
            mis.GetComponent<Rigidbody>().velocity = Vector3.up * 4f;
            mis.GetComponent<MissileCtrl>().tr = CurPerson;
            GameObject mis2 = PoolManager.poolmanager.GetMissile();
            mis2.transform.position = MissilePos[1].position;
            mis2.SetActive(true);
            mis2.GetComponent<MissileCtrl>().tr = CurPerson;
            mis2.GetComponent<Rigidbody>().velocity = Vector3.up * 4f;

            yield return new WaitForSeconds(0.5f);
            Vector3 CurPerson1 = Person.transform.position;
            ShowAttackRange(CurPerson1, Missiledist);

            GameObject mis3 = PoolManager.poolmanager.GetMissile();
            mis3.transform.position = MissilePos[0].position;
            mis3.SetActive(true);
            mis3.GetComponent<Rigidbody>().velocity = Vector3.up * 4f;
            mis3.GetComponent<MissileCtrl>().tr = CurPerson1;
            GameObject mis4 = PoolManager.poolmanager.GetMissile();
            mis4.transform.position = MissilePos[1].position;
            mis4.SetActive(true);
            mis4.GetComponent<Rigidbody>().velocity = Vector3.up * 4f;
            mis4.GetComponent<MissileCtrl>().tr = CurPerson1;

            yield return new WaitForSeconds(0.5f);
            Vector3 CurPerson2 = Person.transform.position;
            ShowAttackRange(CurPerson2, Missiledist);

            GameObject mis5 = PoolManager.poolmanager.GetMissile();
            mis5.transform.position = MissilePos[0].position;
            mis5.SetActive(true);
            mis5.GetComponent<Rigidbody>().velocity = Vector3.up * 4f;
            mis5.GetComponent<MissileCtrl>().tr = CurPerson2;
            GameObject mis6 = PoolManager.poolmanager.GetMissile();
            mis6.transform.position = MissilePos[1].position;
            mis6.SetActive(true);
            mis6.GetComponent<Rigidbody>().velocity = Vector3.up * 4f;
            mis6.GetComponent<MissileCtrl>().tr = CurPerson2;

            Collider[] objs = Physics.OverlapSphere(CurPerson, Missiledist * 0.5f);
            foreach (Collider obj in objs)
            {
                if (obj.tag == "Player")
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                    yield return new WaitForSeconds(0.03f);
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
            yield return new WaitForSeconds(0.5f);
            objs = Physics.OverlapSphere(CurPerson1, Missiledist * 0.5f);
            foreach (Collider obj in objs)
            {
                if (obj.tag == "Player")
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                    yield return new WaitForSeconds(0.03f);
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
            yield return new WaitForSeconds(0.5f);
            objs = Physics.OverlapSphere(CurPerson2, Missiledist * 0.5f);
            foreach (Collider obj in objs)
            {
                if (obj.tag == "Player")
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                    yield return new WaitForSeconds(0.03f);
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(1f);
            agent.isStopped = false;
            gameObject.GetComponent<ActiveData>().Attackactive = false;
        }
        else if (attpa == 2)
        {
            yield return new WaitForSeconds(1f);
            Vector3 att1 = transform.position;
            Vector3 att2 = transform.position + (transform.forward * 4f);
            Vector3 att3 = transform.position + (transform.forward * 8f);
            ShowAttackRange(att1, Cannondist);
            ShowAttackRange(att2, Cannondist);
            ShowAttackRange(att3, Cannondist);
            yield return new WaitForSeconds(0.5f);
            GetComponent<CapsuleCollider>().isTrigger = true;
            _Ani.SetBool("IsRunAtt0",true);
            agent.isStopped = false;
            agent.destination = transform.position+(transform.forward * 8f);
            agent.speed = 8f;
            Collider[] objs = Physics.OverlapSphere(att1, Cannondist * 0.5f);
            foreach (Collider obj in objs)
            {
                if (obj.tag == "Player")
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
            yield return new WaitForSeconds(0.5f);
            objs = Physics.OverlapSphere(att2, Cannondist * 0.5f);
            foreach (Collider obj in objs)
            {
                if (obj.tag == "Player")
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
            yield return new WaitForSeconds(0.5f);
            objs = Physics.OverlapSphere(att3, Cannondist * 0.5f);
            foreach (Collider obj in objs)
            {
                if (obj.tag == "Player")
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
            yield return new WaitForSeconds(0.5f);
            agent.velocity = Vector3.zero;
            agent.speed = 3.5f;
            agent.isStopped = true;
            _Ani.SetBool("IsRunAtt0", false);
            GetComponent<CapsuleCollider>().isTrigger = false;
            yield return new WaitForSeconds(1f);
            agent.isStopped = false;
            gameObject.GetComponent<ActiveData>().Attackactive = false;
        }
    }
    IEnumerator DoHpAttack(GameObject Person)
    {
        Vector3 Personorigin = Person.transform.position;
        gameObject.GetComponent<ActiveData>().Attackactive = true;
        transform.LookAt(Personorigin);
        agent.isStopped = true;

        yield return new WaitForSeconds(0.5f);
        _Ani.SetTrigger("IsAttack0");
        Vector3 dir1 = (Person.transform.position - transform.position).normalized;
        Vector3 dir2 = new Vector3(dir1.x, 0, dir1.z);//점프했을때 y축을 각도계산하지않기위해 추가
        AttackRange();//공격범위 표시
        Collider[] objs = Physics.OverlapSphere(transform.position, attackdist);
        foreach (Collider obj in objs)
        {
            if (obj.tag == "Player")
            {
                if (Vector3.Angle(transform.forward, dir2) < attackangle * 0.5f)
                {
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                    yield return new WaitForSeconds(0.03f);
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
        }
        ShowAttackRange(Personorigin, Cannondist);
        yield return new WaitForSeconds(0.5f);
        _Ani.SetTrigger("IsCannon0");
        objs = Physics.OverlapSphere(Personorigin, Cannondist * 0.5f);
        foreach (Collider obj in objs)
        {
            if (obj.tag == "Player")
            {
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                yield return new WaitForSeconds(0.03f);
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
            }
        }
        yield return new WaitForSeconds(0.47f);
        objs = Physics.OverlapSphere(Personorigin, Cannondist * 0.5f);
        foreach (Collider obj in objs)
        {
            if (obj.tag == "Player")
            {
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                yield return new WaitForSeconds(0.03f);
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
            }
        }
        yield return new WaitForSeconds(0.47f);
        _Ani.SetTrigger("IsMissile0");
        Vector3 CurPerson = Person.transform.position;
        ShowAttackRange(CurPerson, Missiledist);
        yield return new WaitForSeconds(0.5f);
        Vector3 CurPerson1 = Person.transform.position;
        ShowAttackRange(CurPerson1, Missiledist);
        yield return new WaitForSeconds(0.5f);
        Vector3 CurPerson2 = Person.transform.position;
        ShowAttackRange(CurPerson2, Missiledist);
        objs = Physics.OverlapSphere(CurPerson, Missiledist * 0.5f);
        foreach (Collider obj in objs)
        {
            if (obj.tag == "Player")
            {
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                yield return new WaitForSeconds(0.03f);
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
            }
        }
        yield return new WaitForSeconds(0.5f);
        objs = Physics.OverlapSphere(CurPerson1, Missiledist * 0.5f);
        foreach (Collider obj in objs)
        {
            if (obj.tag == "Player")
            {
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                yield return new WaitForSeconds(0.03f);
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
            }
        }
        yield return new WaitForSeconds(0.5f);
        objs = Physics.OverlapSphere(CurPerson2, Missiledist * 0.5f);
        foreach (Collider obj in objs)
        {
            if (obj.tag == "Player")
            {
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                yield return new WaitForSeconds(0.03f);
                obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
            }
        }
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
        gameObject.GetComponent<ActiveData>().Attackactive = false;
    }
    void ShowAttackRange(Vector3 whereatt, float dist)
    {
        GameObject ran = PoolManager.poolmanager.GetCircleRange();
        ran.transform.position = whereatt + new Vector3(0f, 0.1f, 0f);
        ran.GetComponent<SpriteRenderer>().size = new Vector2(dist, dist);
        ran.SetActive(true);
    }
    void AttackRange()//공격범위 표시
    {
        float angle = transform.eulerAngles.y;
        float attackangle1 = attackangle / 2 + angle;
        float attackangle2 = -attackangle / 2 + angle;
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + attackdist * Mathf.Sin((attackangle1) * Mathf.Deg2Rad), 0, transform.position.z + attackdist * Mathf.Cos((attackangle1) * Mathf.Deg2Rad)), Color.green, 3f);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + attackdist * Mathf.Sin((attackangle2) * Mathf.Deg2Rad), 0, transform.position.z + attackdist * Mathf.Cos((attackangle2) * Mathf.Deg2Rad)), Color.green, 3f);
    }
}
