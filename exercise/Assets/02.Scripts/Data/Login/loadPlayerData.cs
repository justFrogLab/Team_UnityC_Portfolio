using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class loadPlayerData : MonoBehaviour
{
    [SerializeField] playerLogin playerLogin;

    private string playerGetIndex = "http://175.125.251.136/unity/getIndex.php";
    public int index;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        playerLogin = GetComponent<playerLogin>();
    }


    private void Update()
    {
        if (playerLogin.loginSucess == false) return;
        else
        {
            StartCoroutine(activeCorutine());
            playerLogin.loginSucess = false;
        }

    }


    #region 코루틴 순차 실행
    IEnumerator activeCorutine()
    {
        yield return StartCoroutine(getlocalIndex());  // 로컬 인덱스 획득
    }
    #endregion
    #region 인덱스 조회
    IEnumerator getlocalIndex()
    {
        string post_url = playerGetIndex
                   + "?id=" + UnityWebRequest.EscapeURL(playerLogin.id)
                   + "&pw=" + UnityWebRequest.EscapeURL(playerLogin.pw);

        UnityWebRequest hs_get = UnityWebRequest.Get(post_url);
        yield return hs_get.SendWebRequest();

        if (hs_get.error != null)
            Debug.Log("There was an error posting the high score: " + hs_get.error);
        else
        {
            string dataText = hs_get.downloadHandler.text;
            index = int.Parse(dataText);
        }
    }
    #endregion
}
