using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerLogin : playerData
{
    [HideInInspector] [SerializeField] InputField idInputField;
    [HideInInspector] [SerializeField] InputField pwInputField;

    [HideInInspector] [SerializeField] Button loginBtn;
    [HideInInspector] [SerializeField] Button signUpBtn;

    [HideInInspector] [SerializeField] CanvasGroup alertWindowCanvasGroup;
    [HideInInspector] [SerializeField] CanvasGroup siginUpWindow;
    [HideInInspector] [SerializeField] Text loginAlertText;

    [SerializeField] CanvasGroup block;

    public bool loginSucess = false;
    private string secretKey = "UnityC";
    private string loginPlayerURL = "http://175.125.251.136/unity/signLogin.php";

    public string id = "";      // 아이디
    public string pw = "";      // 비번

    private void Awake()
    {
        loginBtn.onClick.AddListener(() => loginFunc());
        signUpBtn.onClick.AddListener(() => signupFunc());
    }

    private void Start()
    {
        idInputField.caretBlinkRate = 1f;
        pwInputField.caretBlinkRate = 1f;
    }
    public void loginFunc()
    {
        StartCoroutine(loginPlayer());
    }

    public void signupFunc()
    {
        StartCoroutine(signUpWindowOn());
    }

    IEnumerator alertWindow()
    {
        alertWindowCanvasGroup.alpha = 1;
        alertWindowCanvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(1.5f);
        alertWindowCanvasGroup.alpha = 0;
        alertWindowCanvasGroup.blocksRaycasts = false;
    }

    IEnumerator signUpWindowOn()
    {
        yield return null;
        siginUpWindow.alpha = 1;
        siginUpWindow.blocksRaycasts = true;
    }

    IEnumerator load()
    {
        yield return null;
        SceneManager.LoadScene(base.scence);
    }

    IEnumerator loginPlayer()
    {

        id = idInputField.text;      // 아이디
        pw = pwInputField.text;      // 비번

        string check_url = loginPlayerURL
            + "?id=" + UnityWebRequest.EscapeURL(id)
            + "&pw=" + UnityWebRequest.EscapeURL(pw);

        UnityWebRequest hs_post = UnityWebRequest.Post(check_url, "");


        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);


        idInputField.text = "";
        pwInputField.text = "";
        StartCoroutine(checkPlayerInfo(check_url));

    }

    IEnumerator checkPlayerInfo(string check_url)
    {
        loginSucess = false;


        UnityWebRequest hs_get = UnityWebRequest.Get(check_url);
        yield return hs_get.SendWebRequest();

        if (hs_get.error != null)
            Debug.Log("There was an error posting the high score: " + hs_get.error);
        else
        {
            string dataText = hs_get.downloadHandler.text;


            if (dataText == "sucess")
            {
                loginSucess = true;
            }

            else if (dataText == "pw_fail" || dataText == "id_fail")
            {
                loginAlertText.text = "비밀번호를 확인해주세요.";
                StartCoroutine(alertWindow());

            }
            //MatchCollection mc = Regex.Matches(dataText, @"\n");

            //// 로그인 성공
            //if (mc.Count > 3)
            //{
            //    
            //    playerIndex = Int32.Parse(dataText.Split('\n')[mc.Count - 1]);

            //}
            //else if(mc.Count > 0)
            //{
            //    Debug.Log("id 혹은 pw를 확인해주세요.");
            //    
            //    //string[] splitData = Regex.Split(dataText, @"\n");
            //}
        }

        if (loginSucess)
        {
            block.blocksRaycasts = true;
            #region 테스트 작업 완료 이후 실행되야 하는 지점
            // 작업 완료 이후에 해야 되는 작업
            // setPlayerIndex(playerIndex);
            #endregion

          //  StartCoroutine(load());
        } 
    }
    #region Hash 관리자
    public string HashInput(string input)
    {
        SHA256Managed hm = new SHA256Managed();
        byte[] hashValue =
            hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));
        string hash_convert =
            BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        return hash_convert;
    }
    #endregion
}

