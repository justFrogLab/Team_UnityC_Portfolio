using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerLogin : MonoBehaviour
{
    [SerializeField] InputField idInputField;
    [SerializeField] InputField pwInputField;

    [SerializeField] Button loginBtn;
    [SerializeField] Button signUpBtn;
    
    private string secretKey = "UnityC";
    private string addPlayerURL = "http://localhost:8080/player/playerInfo.php";
    private string loginPlayerURL = "http://localhost:8080/player/playerInfoCheck.php";

    private void Awake()
    {
        loginBtn.onClick.AddListener(() => loginFunc());
        signUpBtn.onClick.AddListener(() => signupFunc());
    }

    public void loginFunc()
    {
        StartCoroutine(loginPlayer());
    }

    public void signupFunc()
    {

        StartCoroutine(sendPlayerInfo());

    }

    IEnumerator loginPlayer()
    {

        string id = idInputField.text;      // 아이디
        string pw = pwInputField.text;      // 비번

        string hash = HashInput(id + pw + secretKey);
        string post_url = loginPlayerURL
            + "?id=" + UnityWebRequest.EscapeURL(id)
            + "&pw=" + UnityWebRequest.EscapeURL(pw)
            + "&hash=" + hash;

        Debug.Log(post_url);
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, hash);


        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);


        idInputField.text = "";
        pwInputField.text = "";
        Debug.Log("로그인 완료");

    }

    IEnumerator sendPlayerInfo()
    {
        string id = idInputField.text;      // 아이디
        string pw = pwInputField.text;      // 비번

        string hash = HashInput(id + pw + secretKey);
        string post_url = addPlayerURL
            + "?id=" + UnityWebRequest.EscapeURL(id)
            + "&pw=" + UnityWebRequest.EscapeURL(pw)
            + "&mainIndex=" + UnityWebRequest.EscapeURL("NULL")
            + "&hash=" + hash;

        Debug.Log(post_url);
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, hash);


        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);


        idInputField.text = "";
        pwInputField.text = "";
        Debug.Log("회원가입 완료");
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

