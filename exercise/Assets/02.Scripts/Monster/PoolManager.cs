using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Area1
    #region Area1_1관련 함수
    [SerializeField] GameObject Area1_1Monster; //Area1 몬스터오브젝트(끌어넣음)
    [SerializeField] GameObject Area1_1MainPoint;//Area1 리젠포인트 부모(끌어넣음)
    Transform[] Area1_1Points; //Area1 리젠포인트 위치
    List<GameObject> Area1_1Pool = new List<GameObject>();//Area1 몬스터 리스트풀
    GameObject Area1_1Mons;//몬스터 동적할당 오브젝트(함수에서 초기화)
    GameObject Area1_1MonsterGroup;//몬스터 부모오브젝트(함수에서 초기화)
    #endregion
    #region Area1_2관련 함수
    [SerializeField] GameObject Area1_2Monster; //Area1 몬스터오브젝트(끌어넣음)
    [SerializeField] GameObject Area1_2MainPoint;//Area1 리젠포인트 부모(끌어넣음)
    Transform[] Area1_2Points; //Area1 리젠포인트 위치
    List<GameObject> Area1_2Pool = new List<GameObject>();//Area1 몬스터 리스트풀
    GameObject Area1_2Mons;
    GameObject Area1_2MonsterGroup;
    #endregion
    #endregion
    #region Area2
    #region Area2_1관련 함수
    [SerializeField] GameObject Area2_1Monster; //Area2 몬스터오브젝트(끌어넣음)
    [SerializeField] GameObject Area2_1MainPoint;//Area2 리젠포인트 부모(끌어넣음)
    Transform[] Area2_1Points; //Area2 리젠포인트 위치
    List<GameObject> Area2_1Pool = new List<GameObject>();//Area2 몬스터 리스트풀
    GameObject Area2_1Mons;
    GameObject Area2_1MonsterGroup;
    #endregion
    #region Area2_2관련 함수
    #region Area2_2노말몬스터
    [SerializeField] GameObject Area2_2Monster; //Area2 몬스터오브젝트(끌어넣음)
    [SerializeField] GameObject Area2_2MainPoint;//Area2 리젠포인트 부모(끌어넣음)
    Transform[] Area2_2Points; //Area2 리젠포인트 위치
    List<GameObject> Area2_2Pool = new List<GameObject>();//Area2 몬스터 리스트풀
    GameObject Area2_2Mons;
    GameObject Area2_2MonsterGroup;
    #endregion
    #region Area2_2챔피언몬스터
    [SerializeField] GameObject Area2_2ChampMonster; //Area2 몬스터오브젝트(끌어넣음)
    [SerializeField] GameObject Area2_2ChampMainPoint;//Area2 리젠포인트 부모(끌어넣음)
    Transform[] Area2_2ChampPoints; //Area2 리젠포인트 위치
    List<GameObject> Area2_2ChampPool = new List<GameObject>();//Area2 몬스터 리스트풀
    GameObject Area2_2ChampMons;
    GameObject Area2_2ChampMonsterGroup;
    #endregion
    #endregion
    #region Area2_3관련 함수
    [SerializeField] GameObject Area2_3BossMonster; //Area2 몬스터오브젝트(끌어넣음)
    [SerializeField] GameObject Area2_3BossMainPoint;//Area2 리젠포인트 부모(끌어넣음)
    Transform[] Area2_3BossPoints; //Area2 리젠포인트 위치
    List<GameObject> Area2_3BossPool = new List<GameObject>();//Area2 몬스터 리스트풀
    GameObject Area2_3BossMons;
    GameObject Area2_3BossMonsterGroup;
    #endregion
    #endregion
    #region 디테일 정보
    float NormalCreateTime = 5f;//노말몬스터 리젠 시간
    float ChampCreateTime = 10f;//챔피언몬스터 리젠 시간
    float BossCreateTime = 30f;//보스몬스터 리젠 시간
    WaitForSeconds NormalWaitForSeconds; //노말몬스터 코루틴 변수지정
    WaitForSeconds ChampWaitForSeconds; //챔피언몬스터 코루틴 변수지정
    WaitForSeconds BossWaitForSeconds; //보스몬스터 코루틴 변수지정
    bool isGameOver = false; //코루틴 무한으로 돌리기위한 변수
    #endregion

    void Awake()
    {
        NormalWaitForSeconds = new WaitForSeconds(NormalCreateTime);
        ChampWaitForSeconds = new WaitForSeconds(ChampCreateTime);
        BossWaitForSeconds = new WaitForSeconds(BossCreateTime);
        #region Area1
        #region Area1_1 몬스터 오브젝트풀
        Area1_1Points = Area1_1MainPoint.GetComponentsInChildren<Transform>();
        CreateMonsterPool(Area1_1Points, Area1_1MonsterGroup, "Area1_1MonsterGroup", Area1_1Mons, Area1_1Monster, "Area1_1Monster", Area1_1Pool);
        #endregion
        #region Area1_2 몬스터 오브젝트풀
        Area1_2Points = Area1_2MainPoint.GetComponentsInChildren<Transform>();
        CreateMonsterPool(Area1_2Points, Area1_2MonsterGroup, "Area1_2MonsterGroup", Area1_2Mons, Area1_2Monster, "Area1_2Monster", Area1_2Pool);
        #endregion
        #endregion
        #region Area2
        #region Area2_1 몬스터 오브젝트풀
        Area2_1Points = Area2_1MainPoint.GetComponentsInChildren<Transform>();
        CreateMonsterPool(Area2_1Points, Area2_1MonsterGroup, "Area2_1MonsterGroup", Area2_1Mons, Area2_1Monster, "Area2_1Monster", Area2_1Pool);
        #endregion
        #region Area2_2 몬스터 오브젝트풀
        Area2_2Points = Area2_2MainPoint.GetComponentsInChildren<Transform>();
        CreateMonsterPool(Area2_2Points, Area2_2MonsterGroup, "Area2_2MonsterGroup", Area2_2Mons, Area2_2Monster, "Area2_2Monster", Area2_2Pool);
        #endregion
        #region Area2_2 챔피언 몬스터 오브젝트풀
        Area2_2ChampPoints = Area2_2ChampMainPoint.GetComponentsInChildren<Transform>();
        CreateMonsterPool(Area2_2ChampPoints, Area2_2ChampMonsterGroup, "Area2_2ChampMonsterGroup", Area2_2ChampMons, Area2_2ChampMonster, "Area2_2ChampMonster", Area2_2ChampPool);
        #endregion
        #region Area2_3 보스 몬스터 오브젝트풀
        Area2_3BossPoints = Area2_3BossMainPoint.GetComponentsInChildren<Transform>();
        CreateMonsterPool(Area2_3BossPoints, Area2_3BossMonsterGroup, "Area2_3BossMonsterGroup", Area2_3BossMons, Area2_3BossMonster, "Area2_3BossMonster", Area2_3BossPool);
        #endregion
        #endregion
        StartCoroutine(CreateMonster());
    }

    void CreateMonsterPool(Transform[] Points,GameObject MonsterGroup,string Groupname,GameObject Mons,GameObject MonPrefab,string Monname,List<GameObject> Pool)
    {//  몬스터풀생성 함수(젠포인트배열      ,부모오브젝트           ,부모오브젝트이름,동적할당변수명 ,몬스터프리팹        ,몬스터이름    ,몬스터풀)
        MonsterGroup = new GameObject(Groupname);
        for (int i=0;i< Points.Length-1;i++)
        {
            Mons = Instantiate(MonPrefab, MonsterGroup.transform);
            Mons.name = Monname + i.ToString();
            Mons.SetActive(true);
            Mons.transform.position = Points[i + 1].position;
            Pool.Add(Mons);
        }
    }

    IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {
            yield return null;
            if (isGameOver) yield break;
            #region Area1
            #region Area1_1 몬스터 리젠기능
            int Area1_1Trans = 0;//몬스터가 태어날 위치 저장변수
            ActivePool(Area1_1Pool, Area1_1Trans, Area1_1Points, NormalWaitForSeconds);
            Area1_1Trans = 0;
            #endregion
            #region Area1_2 몬스터 리젠기능
            int Area1_2Trans = 0;//몬스터가 태어날 위치 저장변수
            ActivePool(Area1_2Pool, Area1_2Trans, Area1_2Points, NormalWaitForSeconds);
            Area1_2Trans = 0;
            #endregion
            #endregion
            #region Area2
            #region Area2_1 몬스터 리젠기능
            int Area2_1Trans = 0;//몬스터가 태어날 위치 저장변수
            ActivePool(Area2_1Pool, Area2_1Trans, Area2_1Points, NormalWaitForSeconds);
            Area2_1Trans = 0;
            #endregion
            #region Area2_2 몬스터 리젠기능
            int Area2_2Trans = 0;//몬스터가 태어날 위치 저장변수
            ActivePool(Area2_2Pool, Area2_2Trans, Area2_2Points, NormalWaitForSeconds);
            Area2_2Trans = 0;
            #endregion
            #region Area2_2 챔피언 몬스터 리젠기능
            int Area2_2ChampTrans = 0;//몬스터가 태어날 위치 저장변수
            ActivePool(Area2_2ChampPool, Area2_2ChampTrans, Area2_2ChampPoints, ChampWaitForSeconds);
            Area2_2ChampTrans = 0;
            #endregion
            #region Area2_3 보스 몬스터 리젠기능
            int Area2_3BossTrans = 0;//몬스터가 태어날 위치 저장변수
            ActivePool(Area2_3BossPool, Area2_3BossTrans, Area2_3BossPoints, BossWaitForSeconds);
            Area2_3BossTrans = 0;
            #endregion
            #endregion
        }
    }
    void ActivePool(List<GameObject> MonsterPool, int Transname,Transform[] Points,WaitForSeconds WaitTime)
    {//풀링동작함수(몬스터풀                    , 위치저장변수 ,젠포인트배열      ,리젠 시간)
        foreach(GameObject Monster in MonsterPool)
        {
            ++Transname;
            if (Monster.activeSelf == false&&Monster.GetComponent<DieAndAlive>().active==false)
            {
                StartCoroutine(RegenMonster(Monster, Points[Transname], WaitTime));
            }
        }
        
    }
    IEnumerator RegenMonster(GameObject Monster, Transform ActivePoint, WaitForSeconds RegenTime)
    {//리젠 코루틴 함수     (몬스터 객체       , 해당객체의 젠포인트  , 리젠 시간)
        Monster.GetComponent<DieAndAlive>().active = true;
        yield return RegenTime;
        Monster.transform.position = ActivePoint.position;
        Monster.SetActive(true);
        Monster.GetComponent<DieAndAlive>().active = false;
    }
}
