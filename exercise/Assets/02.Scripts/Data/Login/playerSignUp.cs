using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class playerSignUp : MonoBehaviour
{
    private string secretKey = "UnityC";
    private string idCheckUrl = "http://175.125.251.136/unity/idOverlapCheck.php";

    [HideInInspector] [SerializeField] CanvasGroup signupWindow;
    [HideInInspector] [SerializeField] CanvasGroup siginupAlertWindow;
    [HideInInspector] [SerializeField] Text signupAlertMsg;

    [HideInInspector] [SerializeField] InputField idInput;
    [HideInInspector] [SerializeField] InputField pwInput;
    [HideInInspector] [SerializeField] InputField pwConfInput;

    [HideInInspector] [SerializeField] Button cancelBtn;
    [HideInInspector] [SerializeField] Button checkIdBtn;
    [HideInInspector] [SerializeField] Button resetId;
    [HideInInspector] [SerializeField] Button okBtn;


    [HideInInspector] [SerializeField] Text alertId;
    [HideInInspector] [SerializeField] Text alertPw;

    bool idCheckSuccess = false;
    bool pwCheckSuccess = false;

    public bool signup = false;
    public string signupId = "";
    public string signupPw = "";
    private void Awake()
    {
        cancelBtn.onClick.AddListener(() => closeWindow());
        checkIdBtn.onClick.AddListener(() => check_id() );
        resetId.onClick.AddListener(() => reset_id());
        okBtn.onClick.AddListener(() => clieckEnterBtn());
    }

    private void Start()
    {
        idInput.caretBlinkRate = 1f;
        pwInput.caretBlinkRate = 1f;
        pwConfInput.caretBlinkRate = 1f;
        pwConfInput.interactable = false;
    }
    public void closeWindow()
    {
        signupWindow.alpha = 0;
        signupWindow.blocksRaycasts = false;

        idInput.text = "";
        pwInput.text = "";
        pwConfInput.text = "";

        idInput.interactable = true;
        checkIdBtn.interactable = true;
        pwConfInput.interactable = false;

        alertId.text = "";
        alertPw.text = "";

        alertPw.color = Color.red;
        alertPw.color = Color.red;

        idCheckSuccess = false;
        pwCheckSuccess = false;
    }

    public void check_id()
    {
        StartCoroutine(idCheck());
    }

    public void reset_id()
    {
        StartCoroutine(resetidInput());
    }

    public void clieckEnterBtn()
    {
        string msg = "";

        if (idCheckSuccess && pwCheckSuccess)
        {
            msg = "회원 가입 완료";
            signup = true;
            signupId = idInput.text;
            signupPw = pwInput.text;
            StartCoroutine(signupResultAlert(msg));
            closeWindow();
        }

        else
        {
            if (!idCheckSuccess)
            {
                msg = "아이디를 확인해주세요.";
                StartCoroutine(signupResultAlert(msg));
            }

            else if (!pwCheckSuccess)
            {
                msg = "비밀번호를 확인해주세요.";
                StartCoroutine(signupResultAlert(msg));
            }
        }
           
    }

    private void Update()
    {
        passwordCheck();
    }

    public void passwordCheck()
    {
        if (pwInput.isFocused)
        {
            if (pwInput.text == "")
            {
                pwCheckSuccess = false;
                pwConfInput.interactable = false;
                pwConfInput.text = "";
                alertPw.text = "";
                alertPw.color = Color.red;
            }

            else if (pwInput.text != "")
            {
                pwConfInput.interactable = true;
            }

            if (pwConfInput.text == "")
            {
                pwCheckSuccess = false;
                return;
            }

            else if(pwInput.text != pwConfInput.text)
            {
                pwCheckSuccess = false;
                alertPw.color = Color.red;
                alertPw.text = "비밀번호를 확인해주세요.";
            }
        }

        if (pwConfInput.isFocused) // 비밀번호 확인창 누름
        {
            if (pwInput.text == "")     // 아직 비밀번호 입력을 안함
            {
                pwCheckSuccess = false;
                return;
            }

            else if (pwInput.text != pwConfInput.text) // 비밀번호 불일치
            {
                pwCheckSuccess = false;
                alertPw.color = Color.red;
                alertPw.text = "비밀번호를 확인해주세요.";
            }

            else if (pwInput.text == pwConfInput.text) // 비밀번호 일치
            {
                pwCheckSuccess = true;
                alertPw.text = "확인";
                alertPw.color = Color.green;
            }
        }
    }

    IEnumerator signupResultAlert(string msg)
    {
        signupAlertMsg.text = msg;
        siginupAlertWindow.alpha = 1;
        siginupAlertWindow.blocksRaycasts = true;
        yield return new WaitForSeconds(1.0f);
        siginupAlertWindow.alpha = 0;
        siginupAlertWindow.blocksRaycasts = false;
    }

    IEnumerator idCheck()
    {
        #region id 특수 문자검사 
        string idString = idInput.text;
        MatchCollection specialChar = Regex.Matches(idString, @"[^a-zA-Z0-9가-힣]");  // 영어 소 대문자만 
        if (specialChar.Count > 0)
        {
            idInput.text = "";
            alertId.color = Color.red;
            alertId.text = "특수 문자, 한글은 포함 할 수 없습니다.";
            yield break;
        }
        #endregion

        string get_url = idCheckUrl + "?id=" + UnityWebRequest.EscapeURL(idString);
        UnityWebRequest hs_get = UnityWebRequest.Get(get_url);
        yield return hs_get.SendWebRequest();

        if (hs_get.error != null)
            Debug.Log("There was an error posting the high score: " + hs_get.error);

        else
        {
            // 아이디 값 존재하는지에 대한 여부
            string dataText = hs_get.downloadHandler.text;

            if (dataText == "exist")    // 존재 -> 다시 입력
            {
                idCheckSuccess = false;
                alertId.text = "동일한 id가 존재합니다.";
                alertId.color = Color.red;
                idInput.text = "";
            }

            else if (dataText == "null") // 비존재 -> 아이디 고정
            {
                idCheckSuccess = true;
                alertId.text = "사용 가능한 아이디 입니다.";
                alertId.color = Color.green;
                idInput.interactable = false;
                checkIdBtn.interactable = false;
            }
        }
    }

    IEnumerator resetidInput()
    {
        yield return null;
        idInput.text = "";
        alertId.text = "";

        idInput.interactable = true;
        checkIdBtn.interactable = true;

        idCheckSuccess = false;
    }
}
