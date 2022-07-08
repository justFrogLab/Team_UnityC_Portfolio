using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCtrl : playerData
{
    bool IsDie = false;
    private void Start()
    {
        #region 임시 객체 초기화 !!!!! 수정될 예정
        base.testInit();
        #endregion

        tr.position = startPos;     // 자리 초기화
    }

    private void Update()
    {
        updateDmg();
    }

    void updateDmg()
    {
        switch (job)
        {
            // 견습
            case 1:
                damage = inter * 1 + dex * 1 + inter * 1;
                break;

            // 검사
            case 2:
                damage = inter * 1.7833 + dex * 1 + inter * 1;
                break;

            // 총사
            case 3:
                damage = inter * 1 + dex * 1.5 + inter * 1;
                break;
        }
    }

    #region 몬스터 스크립트에서 SendMessasge
    IEnumerator GetDamage(int dmg)
    {
        hpCur = Mathf.Clamp(hpCur - dmg, 0, hpMax);
        yield return null;
        Debug.Log(hpCur);
    }
    #endregion
}
