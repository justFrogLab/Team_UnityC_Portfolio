using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnenableItem : MonoBehaviour
{
    float time = 20f;
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
