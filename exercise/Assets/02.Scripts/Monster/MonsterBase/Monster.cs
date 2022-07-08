using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonsterBaseMethod
{
    public IEnumerator Patrol()//몬스터에서 직접접근해야함(public)
    {//몬스터주변 패트롤 함수
        Vector3 originTr = transform.position;//초기 위치
        Vector3 movePoint;//패트롤할 위치
        agent.enabled = true;
        float DelayTime = 1f;//이동 완료 후 얼마나 대기하는가 총5회 반복
        StartCoroutine(DetectPlayer());//패트롤을 하면서 디텍팅 동시 진행
        WaitForSeconds DelayWait = new WaitForSeconds(DelayTime);//WaitforSeconds 초기화
        while (!IsDie)
        {
            while (gameObject.GetComponent<ActiveData>().Patrolactive==true)
            {
                if (IsDie) yield break;
                if (gameObject.GetComponent<ActiveData>().Patrolactive == false) break;
                if(Vector3.Distance(transform.position, originTr) < 0.1f)
                {
                    _Ani.SetBool("IsTrace", false);
                    _Ani.SetBool("IsWalk", false);
                    agent.isStopped = true;
                    #region 딜레이 함수
                    if (gameObject.GetComponent<ActiveData>().Patrolactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        break;
                    }
                    if (gameObject.GetComponent<ActiveData>().Patrolactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        break;
                    }
                    if (gameObject.GetComponent<ActiveData>().Patrolactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        break;
                    }
                    if (gameObject.GetComponent<ActiveData>().Patrolactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        break;
                    }
                    if (gameObject.GetComponent<ActiveData>().Patrolactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        break;
                    }
                    #endregion
                    agent.isStopped = false;
                    movePoint = transform.position + (transform.forward * Random.Range(-movedist, movedist)) + (transform.right * Random.Range(-movedist, movedist));//패트롤할 위치 랜덤 적용
                    agent.destination = movePoint;
                    _Ani.SetBool("IsWalk", true);
                    transform.LookAt(movePoint);
                    if (gameObject.GetComponent<ActiveData>().Collisionactive == false)
                    {
                        StartCoroutine(CollisionMonster(movePoint, DelayWait, originTr));//패트롤 함수 실행 후 이동중 충돌감지기능 실행
                    }
                }
                yield return null;
            }
            yield return null;
        }
    }
    IEnumerator CollisionMonster(Vector3 movePoint,WaitForSeconds DelayWait,Vector3 originTr)//패트롤이후 동작으로 직접접근할수없는 함수(private)
    {//이동중 충돌감지기능      (패트롤할 위치    ,도착후 대기시간 5회반복 ,몬스터 초기위치)
        RaycastHit hit;//Ray를 맞은 대상
        Vector3 origin;//Ray를 쏘는 위치
        gameObject.GetComponent<ActiveData>().Collisionactive = true;//코루틴을 중복 실행시키지 않기위한 bool변경
        gameObject.GetComponent<ActiveData>().Patrolactive = false;//코루틴을 중복 실행시키지 않기위한 bool변경
        while (!IsDie)
        {
            if (IsDie) yield break;
            yield return null;
            origin = transform.position + (transform.up * 0.5f);
            Debug.DrawRay(origin, (movePoint - transform.position), Color.green);
            if (Physics.Raycast(origin, movePoint - transform.position, out hit, movedist))
            {
                if (Vector3.Distance(transform.position, hit.collider.transform.position) < 3f)
                {
                    _Ani.SetBool("IsTrace", false);
                    _Ani.SetBool("IsWalk", false);
                    agent.isStopped = true;
                    #region 딜레이 함수
                    if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        yield break;
                    }
                    if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        yield break;
                    }
                    if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        yield break;
                    }
                    if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        yield break;
                    }
                    if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                    {
                        yield return DelayWait;
                    }
                    else
                    {
                        yield break;
                    }
                    #endregion
                    agent.isStopped = false;
                    agent.destination = originTr;
                    _Ani.SetBool("IsWalk", true);
                    transform.LookAt(originTr);
                    gameObject.GetComponent<ActiveData>().Collisionactive = false;
                    gameObject.GetComponent<ActiveData>().Patrolactive = true;
                    yield break;
                }
            }
            if(Vector3.Distance(transform.position, movePoint)<0.1f)
            {
                _Ani.SetBool("IsTrace", false);
                _Ani.SetBool("IsWalk", false);
                agent.isStopped = true;
                #region 딜레이함수
                if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                {
                    yield return DelayWait;
                }
                else
                {
                    yield break;
                }
                if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                {
                    yield return DelayWait;
                }
                else
                {
                    yield break;
                }
                if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                {
                    yield return DelayWait;
                }
                else
                {
                    yield break;
                }
                if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                {
                    yield return DelayWait;
                }
                else
                {
                    yield break;
                }
                if (gameObject.GetComponent<ActiveData>().Collisionactive == true)
                {
                    yield return DelayWait;
                }
                else
                {
                    yield break;
                }
                #endregion
                agent.isStopped = false;
                agent.destination = originTr;
                _Ani.SetBool("IsWalk", true);
                transform.LookAt(originTr);
                gameObject.GetComponent<ActiveData>().Collisionactive = false;
                gameObject.GetComponent<ActiveData>().Patrolactive = true;
                yield break;
            }
        }
    }
    public IEnumerator DetectPlayer()//몬스터에서 직접접근해야함(public)
    {//몬스터주변플레이어 감지함수
        Collider[] objs;//감지된 객체를 담기위한 콜라이더배열
        RaycastHit hit;//Ray를 맞은 대상
        Vector3 origin;//Ray를 쏘는 위치
        Vector3 originTr = transform.position;//몬스터의 초기위치
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
                        origin= transform.position + (transform.up * 0.5f);
                        if (Physics.Raycast(origin, obj.gameObject.transform.position - transform.position, out hit, detectdist))
                        {
                            Debug.DrawRay(origin, (obj.gameObject.transform.position - transform.position), Color.green);
                            if (hit.collider.CompareTag("Player"))
                            {
                                StartCoroutine(Trace(originTr,obj.gameObject));
                                break;//플레이어가 주변에 여러명이있어도 처음 발견한 플레이어만 감지 후 foreach종료
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator Trace(Vector3 originTr,GameObject Person)//피격 이후 / 탐지 이후 동작으로 직접접근할수없는 함수(private)
    {//추적하는 함수                (초기위치        ,타겟 위치)
        RaycastHit hit;//Ray를 맞은대상
        Vector3 origin;//Ray를 쏘는 위치
        agent.enabled = true;
        agent.isStopped = false;
        float Maxdist;//추적할수있는 최대거리
        gameObject.GetComponent<ActiveData>().Collisionactive = false;//코루틴을 중복 실행시키지 않기위한 bool변경
        gameObject.GetComponent<ActiveData>().Patrolactive = false;//코루틴을 중복 실행시키지 않기위한 bool변경
        gameObject.GetComponent<ActiveData>().Detectactive = false;//코루틴을 중복 실행시키지 않기위한 bool변경
        gameObject.GetComponent<ActiveData>().Traceactive = true;//코루틴을 중복 실행시키지 않기위한 bool변경
        while (!IsDie&&gameObject.GetComponent<ActiveData>().Traceactive)
        {
            if (IsDie) yield break;
            if (!gameObject.GetComponent<ActiveData>().Traceactive) yield break;
            yield return null;
            if (gameObject.GetComponent<ActiveData>().Attackactive) continue;
            _Ani.SetBool("IsWalk", false);
            _Ani.SetBool("IsTrace", true);
            origin = transform.position + (transform.up * 1f);
            Maxdist = Vector3.Distance(originTr, transform.position);
            agent.destination = Person.transform.position;
            transform.LookAt(Person.transform.position);
            if (Physics.Raycast(origin, Person.transform.position - transform.position, out hit, detectdist))
            {
                Debug.DrawRay(origin, (Person.transform.position - transform.position), Color.green);
                if (!hit.collider.CompareTag("Player")&&!hit.collider.CompareTag("MONSTER"))//Ray에 닿은 대상이 player나 monster가 아니면 초기위치 복귀
                {
                    _Ani.SetBool("IsTrace", false);
                    StartCoroutine(EndPosition(originTr));
                }
            }
            if (Maxdist>10f)
            {
                _Ani.SetBool("IsTrace", false);
                StartCoroutine(EndPosition(originTr));
            }
            else if(Vector3.Distance(transform.position, Person.transform.position)<attackdist)
            {
                _Ani.SetBool("IsTrace", false);
                StartCoroutine(DoAttack(Person));
            }
        }
    }

    IEnumerator DoAttack(GameObject Person)
    {
        Vector3 Personorigin = Person.transform.position;
        gameObject.GetComponent<ActiveData>().Attackactive = true;
        transform.LookAt(Personorigin);
        agent.isStopped = true;
        _Ani.SetTrigger("IsAttack");
        yield return new WaitForSeconds(0.5f);
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
                    Debug.Log("플레이어한태피해줌");
                    obj.gameObject.SendMessage("GetDamage", Damage - obj.gameObject.GetComponent<playerCtrl>().def, SendMessageOptions.DontRequireReceiver);//플레이어 히트모션동작
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        agent.isStopped = false;
        gameObject.GetComponent<ActiveData>().Attackactive = false;
    }

    void AttackRange()//공격범위 표시
    {
        float angle = transform.eulerAngles.y;
        float attackangle1 = attackangle / 2 + angle;
        float attackangle2 = -attackangle / 2 + angle;
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + attackdist * Mathf.Sin((attackangle1) * Mathf.Deg2Rad), 0, transform.position.z + attackdist * Mathf.Cos((attackangle1) * Mathf.Deg2Rad)), Color.green, 3f);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + attackdist * Mathf.Sin((attackangle2) * Mathf.Deg2Rad), 0, transform.position.z + attackdist * Mathf.Cos((attackangle2) * Mathf.Deg2Rad)), Color.green, 3f);
    }

    IEnumerator EndPosition(Vector3 originTr)//추적이후 동작으로 직접접근할수없는 함수(private)
    {//추적도중 복귀 함수  (초기위치) -> 복귀중 무적적용 필요
        agent.destination = originTr;
        transform.LookAt(originTr);
        _Ani.SetBool("IsWalk", true);
        gameObject.GetComponent<ActiveData>().Traceactive = false;//코루틴을 중복 실행시키지 않기위한 bool변경
        gameObject.GetComponent<ActiveData>().ActiveEndPosition = true;//코루틴을 중복 실행시키지 않기위한 bool변경
        while (gameObject.GetComponent<ActiveData>().ActiveEndPosition)
        {
            if (!gameObject.GetComponent<ActiveData>().ActiveEndPosition) yield break;
            yield return null;
            if (Vector3.Distance(transform.position, originTr) < 0.1f)
            {
                _Ani.SetBool("IsWalk", false);
                gameObject.GetComponent<ActiveData>().ActiveEndPosition = false;
                gameObject.GetComponent<ActiveData>().Detectactive = true;
                gameObject.GetComponent<ActiveData>().Collisionactive = false;
                gameObject.GetComponent<ActiveData>().Patrolactive = true;
            }
        }
    }
}
