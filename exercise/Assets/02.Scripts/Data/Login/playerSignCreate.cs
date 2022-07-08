using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerSignCreate : MonoBehaviour
{
    [SerializeField] playerSignUp playerSignUp;

    private string playerAddIndex = "http://175.125.251.136/unity/playerSetIndex.php";
    private string playerGetIndex = "http://175.125.251.136/unity/getIndex.php";

    private string playerSpaceAdd = "http://175.125.251.136/unity/initAdd/playerSpaceAdd.php";
    private string playerStatAdd = "http://175.125.251.136/unity/initAdd/playerStatusAdd.php";
    private string playerExtraAdd = "http://175.125.251.136/unity/initAdd/playerExtraAdd.php";
    private string playerEquipAdd = "http://175.125.251.136/unity/initAdd/playerEquipAdd.php";
    private string playerInventoryAdd = "http://175.125.251.136/unity/initAdd/playerInventoryAdd.php";
    private string playerSkillAdd = "http://175.125.251.136/unity/initAdd/playerSkillAdd.php";


    private string _id = "";
    private string _pw = "";

    // 기초 초기화 정보
    private string index = "";
    private Vector3 initPos = Vector3.zero;
    private int initScence = 2;
    private void Awake()
    {
        playerSignUp = GetComponent<playerSignUp>();
    }

    private void Update()
    {
        if (playerSignUp.signup == false) return;
        else
        {
            StartCoroutine(activeCorutine());   
            playerSignUp.signup = false;
        }

    }

    IEnumerator activeCorutine()
    {
        yield return StartCoroutine(initPlayerIndex()); // 인덱스 생성
        yield return StartCoroutine(getlocalIndex());  // 로컬 인덱스 획득
        yield return StartCoroutine(initStat());    // 스탯 정보 생성
        yield return StartCoroutine(initSpace()); // 공간 인덱스 생성
        yield return StartCoroutine(initExtra());   // 부가 정보 생성
        yield return StartCoroutine(initEquip());   // 장비 정보 생성
        yield return StartCoroutine(initPlayerInventory()); // 인벤 정보 생성
        yield return StartCoroutine(initPlayerSkill()); // 스킬 정보 생성
    }


    #region 인덱스 생성
    IEnumerator initPlayerIndex()
    {
        _id = playerSignUp.signupId;      // 아이디
        _pw = playerSignUp.signupPw;      // 비번

        string post_url = playerAddIndex
            + "?id=" + UnityWebRequest.EscapeURL(_id)
            + "&pw=" + UnityWebRequest.EscapeURL(_pw);

        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");


        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);

        yield return getlocalIndex();
    }
    #endregion
    #region 인덱스 조회
    IEnumerator getlocalIndex()
    {
        string post_url = playerGetIndex
                   + "?id=" + UnityWebRequest.EscapeURL(_id)
                   + "&pw=" + UnityWebRequest.EscapeURL(_pw);

        UnityWebRequest hs_get = UnityWebRequest.Get(post_url);
        yield return hs_get.SendWebRequest();

        if (hs_get.error != null)
            Debug.Log("There was an error posting the high score: " + hs_get.error);
        else
        {
            string dataText = hs_get.downloadHandler.text;
            index = dataText;
        }
    }
    #endregion
    #region 스탯 정보 생성
    IEnumerator initStat()
    {
        _id = playerSignUp.signupId;      // 아이디
        _pw = playerSignUp.signupPw;      // 비번

        string post_url = playerStatAdd
            + "?pindex=" + UnityWebRequest.EscapeURL(index)
            + "&hp=" + 100
            + "&mp=" + 100
            + "&str=" + 1
            + "&dex=" + 1
            + "&inter=" + 1
            + "&def=" + 1
            + "&damage=" + 10
            + "&statP=" + 0
            + "&skillP=" + 0
            ;
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");
        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);
    }
    #endregion
    #region 공간 정보 생성
    IEnumerator initSpace()
    {
        _id = playerSignUp.signupId;      // 아이디
        _pw = playerSignUp.signupPw;      // 비번

        string post_url = playerSpaceAdd
            + "?pindex=" + UnityWebRequest.EscapeURL(index)
            + "&scence=" + initScence
            + "&x=" + initPos.x
            + "&y=" + initPos.y
            + "&z=" + initPos.z;
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");
        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);
    }

    #endregion
    #region 부가 정보 생성
    IEnumerator initExtra()
    {
        string post_url = playerExtraAdd
        + "?pindex=" + UnityWebRequest.EscapeURL(index)
        + "&lv=" + 100
        + "&name=" + UnityWebRequest.EscapeURL("basic")
        + "&job=" + 1
        + "&curExp=" + 0;
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");

        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);
    }
    #endregion
    #region 장비 정보 생성
    IEnumerator initEquip()
    {
        string post_url = playerEquipAdd
        + "?pindex=" + UnityWebRequest.EscapeURL(index)
        + "&weapon=" + 0
        + "&defend=" + 0
        + "&acc=" + 0;
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");

        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);
    }
    #endregion
    #region 인벤 정보 생성
    IEnumerator initPlayerInventory()
    {
        string post_url = playerInventoryAdd
        + "?pindex=" + UnityWebRequest.EscapeURL(index)
        + "&gold=" + 0
        + "&item=" + UnityWebRequest.EscapeURL("0_0_0")
        + "&count=" + UnityWebRequest.EscapeURL("0_0_0");
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");

        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);
    }
    #endregion

    #region 스킬 정보 생성
    IEnumerator initPlayerSkill()
    {
        string post_url = playerSkillAdd
        + "?pindex=" + UnityWebRequest.EscapeURL(index)
        + "&skill=" + UnityWebRequest.EscapeURL("0_0_0")
        + "&count=" + UnityWebRequest.EscapeURL("0_0_0");
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");

        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);
    }
    #endregion
}
