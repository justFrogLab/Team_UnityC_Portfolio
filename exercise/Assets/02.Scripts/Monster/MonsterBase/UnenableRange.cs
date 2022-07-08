using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnenableRange : MonoBehaviour
{
    float time = 1f;
    WaitForSeconds disabletime;
    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        disabletime = new WaitForSeconds(time);
        yield return disabletime;
        gameObject.SetActive(false);
    }
}
