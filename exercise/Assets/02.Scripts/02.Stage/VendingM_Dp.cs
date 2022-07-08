using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingM_Dp : MonoBehaviour
{
    public Material _dp1;
    public Material _dp2;
    public Material _dp3;
    [SerializeField]
    private MeshRenderer _renderer;

    void Awake()
    {
        _renderer = transform.GetChild(2).GetComponent<MeshRenderer>();
        _renderer.material = _dp1;
        StartCoroutine(ChangeDP());
    }

    IEnumerator ChangeDP()
    {
        while(true)
        {
            yield return null;
            _renderer.material = _dp1;
            yield return new WaitForSeconds(3.0f);
            _renderer.material = _dp2;
            yield return new WaitForSeconds(3.0f);
            _renderer.material = _dp3;
            yield return new WaitForSeconds(3.0f);
        }
    }

    
}
