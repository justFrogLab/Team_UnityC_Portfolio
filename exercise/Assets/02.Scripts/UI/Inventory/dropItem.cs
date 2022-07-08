using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class dropItem : MonoBehaviour
{
    [SerializeField] InputField countField;
    [SerializeField] Button dropOkBtn;
    [SerializeField] Button exitBtn;

    [SerializeField] CanvasGroup inven_CG;
    CanvasGroup drop_item_CG;
    
    bool dropCGopen = false;

    public int idx = 0;
    int maxCount = 1;
    int nowCount = 1;
    private void Awake()
    {
        drop_item_CG = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        exitBtn.onClick.AddListener(() => exit());
        dropOkBtn.onClick.AddListener(() => ok());
    }

    private void Update()
    {
        // 버리는 아이템 창
        dropCGopen = drop_item_CG.alpha == 1;
        if (!dropCGopen) return;

        // 인벤 클릭 방지
        inven_CG.blocksRaycasts = false;

        // 인벤토리 숫자만 받아오기
        countField.text = Regex.Replace(countField.text, @"[^0-9]", "");
        
        // 인벤토리 최대 카운트
        maxCount = Data_Manager.instance.inven_item_count(idx);

        if (maxCount == 1)
        {
            countField.text = "1";
        }

        // 현재 카운트
        nowCount = int.Parse(countField.text);


        // 현재 카운트가 최대 카운트를 넘겨 버리면
        if (nowCount > maxCount)
        {
            countField.text = maxCount.ToString();
        }

    }

    void ok()
    {
        down();
        // 인벤토리 에서 해당 아이템 값 소거
        for(int i = 0; i < nowCount; i++)
        Data_Manager.instance.useItem(idx);

        // ui 갱신
        UI_Manager.instance.use_item(idx);

    }

    void exit()
    {
        down();
    }

    void down()
    {
        inven_CG.blocksRaycasts = true;
        UI_Manager.instance.pop_stack(drop_item_CG);
    }
}
