using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCtrl : MonoBehaviour
{
    public float upvelo;
    public Vector3 tr;
    bool isremain = false;

    private void OnEnable()
    {
        isremain = true;
        StartCoroutine(LaunchDelay());
        StartCoroutine(Explosion());
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(gomis());

    }
    IEnumerator gomis()
    {
        while(isremain)
        {
            yield return null;
            if(!isremain) yield break;
            Vector3 dir = (tr - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, dir, 0.25f);
            transform.position += transform.up *20f * Time.deltaTime;
        }
        yield return null;
    }
    IEnumerator Explosion()
    {
        while(isremain)
        {
            yield return null;
            if (!isremain) yield break;
            if((tr-transform.position).magnitude<1f)
            {
                isremain = false;
                gameObject.SetActive(false);
            }
        }
        yield return null;
    }
    private void OnDisable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
