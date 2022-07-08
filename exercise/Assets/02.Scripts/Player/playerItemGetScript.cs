using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerItemGetScript : MonoBehaviour
{



    private void Update()
    {
        Collider[] itemcols = Physics.OverlapSphere(transform.position,radius: 5f,1<<8);        // Item 레이어 객체만 수집

        if (itemcols.Length >= 0)     
        {
            //Debug.Log("획득 가능");
            // TODO: 아이템 획득 가능 ui 표시
        }

        if (Input.GetKeyDown(KeyCode.F)) ItemGet(itemcols);

    }

    public void ItemGet(Collider[] cols)
    {
        foreach (Collider col in cols)
        {
            List<int> tempItems = new List<int>();         // int형 list
            string[] itemString = col.name.Split('_');         // string형 배열
            for (int i = 0; i < itemString.Length - 1; i++) tempItems.Add(int.Parse(itemString[i]));    // int형 Parse

            string remain = ""; // 빈 값을 담을 객체
            for (int i = 0; i < tempItems.Count; i++)
            {
                bool isLast = (i == tempItems.Count - 1);
                bool sucess = UI_Manager.instance.sendItemInven(tempItems[i], isLast);   // 전송 결과 판단
                if (!sucess) remain += itemString[i] + "_";        // 전송 결과 : 성공 : 해당 문자열은 지운다
            }

            if (remain == "") Destroy(col.gameObject);  // 전송 성공 : 해당 게임 오브젝트 제거
            else col.gameObject.name = remain + "Item"; // 전송 실패: 해당 게임 오브젝트 이름 변경
        }
    }
}
