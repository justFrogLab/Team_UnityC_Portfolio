using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;


    [SerializeField] CanvasGroup inventoryCG;
    [SerializeField] CanvasGroup dropItemCG;
    [SerializeField] CanvasGroup playerInfoCG;  // 좌 상단 스탯 창
    [SerializeField] CanvasGroup playerEquipCG; // 장비 창에 스탯 같이 표시
    [SerializeField] CanvasGroup itemInfoCG;

    [SerializeField] playerCtrl playerCtrl;
    [SerializeField] Inventory inventory;

    [SerializeField] Equip equip;

    [SerializeField] GameObject hp;
    [SerializeField] GameObject mp;
    [SerializeField] Sprite barFade_sprite;

    List<CanvasGroup> open_UI_stack;

    Text hpText;
    Image hpImg;

    Text mpText;
    Image mpImg;
    #region Awake
    private void Awake()
    {
        instance = this;
        open_UI_stack = new List<CanvasGroup>();



        hpImg = hp.transform.GetChild(1).GetComponent<Image>();
        hpText = hpImg.transform.GetChild(0).GetComponent<Text>();


        mpImg = mp.transform.GetChild(1).GetComponent<Image>();
        mpText = mpImg.transform.GetChild(0).GetComponent<Text>();
    }
    #endregion

    #region Start
    private void Start()
    {
        barFade_sprite = Resources.Load<Sprite>("BarFade");
    }
    #endregion
    #region Update ::: 입력키 매니저
    private void Update()
    {
        input_I();
        input_E();
        input_Escape();
    }
    #endregion

    #region 입력 - I ::: 기능 - 인벤토리
    public void input_I()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool _isclose = (inventoryCG.alpha == 0);        // 닫혀 있는지 확인
            if (_isclose) push_stack(inventoryCG);        // 스택에 밀어 넣음 ( 리스트 형)
            else pop_stack(inventoryCG);     // 스택에서 없앰 (리스트형)
        }
    }
    #endregion

    #region 입력 - E ::: 기능 - 장비창
    public void input_E()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool _isclose = (playerEquipCG.alpha == 0);        // 닫혀 있는지 확인
            if (_isclose) push_stack(playerEquipCG);        // 스택에 밀어 넣음 ( 리스트 형)
            else pop_stack(playerEquipCG);     // 스택에서 없앰 (리스트형)
        }
    }
    #endregion

    #region 입력 - ESC ::: 기능 - 열린 순서대로 닫기
    public void input_Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (open_UI_stack.Count <= 0) return;
            pop_stack(open_UI_stack[open_UI_stack.Count - 1]);
        }
    }
    #endregion

    #region UI 열기 ::: push_stack 에서 제어
    public void open_UI(CanvasGroup CG)
    {
        CG.alpha = 1;
        CG.blocksRaycasts = true;
    }
    #endregion

    #region UI 닫기 ::: ::: push_pop 에서 제어
    public void close_UI(CanvasGroup CG)
    {
        CG.alpha = 0;
        CG.blocksRaycasts = false;
    }
    #endregion

    #region ::: push_stack
    public void push_stack(CanvasGroup CG)
    {
        open_UI(CG);
        open_UI_stack.Add(CG);
    }
    #endregion

    #region ::: pop_stack
    public void pop_stack(CanvasGroup CG)
    {
        close_UI(CG);
        open_UI_stack.Remove(CG);
    }
    #endregion

    #region 인벤토리로 아이템 전송
    public bool sendItemInven(int idx, bool isLast = false)
    {
        int empty = inventory.checkEmpty(); // 빈 공간
        if (empty < 0)
        {
            inventory.updateInvenUI();
            return false; // 인벤토리 빈공간 없음
        }

        // 아이템 타입 검사
        int type
            = (int)csvReader.itemData[csvReader.itemIndexPairs[idx]][csvReader.itemHeaderPairs["itemType"]];


        //Debug.Log(idx);
        if (type != 1)   // 소비 아이템
        {
            int check = inventory.indexCheck(idx);

            if (check != -1) // 동일한 소비 아이템이 존재
            {
                inventory.itemCount[check]++;   // 동일한 소비 아이템의 카운트 증가
            }

            else if (check == -1)
            {   // 동일한 소비 아이템이 존재하지 않음
                inventory.itemIndexs[empty] = idx;  // 빈 칸에 새로운 인덱스 생성 
                inventory.itemCount[empty] = 1; // 빈 칸에 새로운 카운트 증가
            }
        }

        else if (type == 1) // 장비 아이템
        {
            inventory.itemIndexs[empty] = idx;  // 빈 칸에 새로운 인덱스 생성 
            inventory.itemCount[empty] = 1; // 빈 칸에 새로운 카운트 증가
        }



        if (isLast) inventory.updateInvenUI();
        return true;
    }
    #endregion

    #region HP, MP 업데이트
    public void update_hp_mp()
    {
        update_hp();
        update_mp();
    }
    #endregion

    #region HP 업데이트
    public void update_hp()
    {
        hpText.text = playerCtrl.hpMax.ToString() + "/" + playerCtrl.hpCur.ToString();
        hpImg.fillAmount = (float)playerCtrl.hpCur / playerCtrl.hpMax;
    }
    #endregion

    #region MP 업데이트
    public void update_mp()
    {
        mpText.text = playerCtrl.mpMax.ToString() + "/" + playerCtrl.mpCur.ToString();
        mpImg.fillAmount = (float)playerCtrl.mpCur / playerCtrl.mpMax;
    }
    #endregion

    #region 소비 아이템 사용시 ::: 해당 인덱스만 업데이트
    public void use_item(int _idx)
    {
        if (inventory.itemIndexs[_idx] <= 0)
        {
            inventory.slots[_idx].sprite = null;
            inventory.slots[_idx].color = Color.black;
            inventory.slots[_idx].transform.GetChild(0).GetComponent<Text>().text = "";
        }

        else
        {
            inventory.slots[_idx].transform.GetChild(0).GetComponent<Text>().text =
                inventory.itemCount[_idx].ToString();
        }
    }
    #endregion

    #region 장비창 ::: 장비 아이템 사용(클릭)시 ::: 인벤토리 ::: 업데이트
    public void inven_update_click_equip(int thisidx, List<object> itemInfo)
    {
        inventory.slots[thisidx].sprite =
            (Sprite)itemInfo[csvReader.itemHeaderPairs["icon"]];

        inventory.slots[thisidx].color = Color.white;
    }
    #endregion

    #region 장비창 ::: 장비 아이템 사용(클릭)시 ::: 장비창 ::: 업데이트
    public void equip_update_click_equip(int thisidx)
    {
        equip.equipItemSlot[thisidx].GetComponent<Image>().sprite = barFade_sprite;
    }
    #endregion

    #region 인벤토리창 ::: 장비 아이템 장착에 성공! 해당 인덱스만 업데이트 할게요.
    public void mount_update_item(int part, List<object> partData)
    {
        equip.equipItemSlot[part - 1].GetComponent<Image>().sprite
            = (Sprite)partData[csvReader.itemHeaderPairs["icon"]];
    }
    #endregion

    #region
    public void exchange_clik_slot(int idx)
    {
 

        int mIdx = inventory.itemIndexs[idx];
        List<object> temInfo = csvReader.itemData[csvReader.itemIndexPairs[mIdx]];
        int temType = (int)temInfo[csvReader.itemHeaderPairs["itemType"]];

        if (inventory.itemIndexs[idx] == 0)
        {
            inventory.slots[idx].sprite = null;
            inventory.slots[idx].color = Color.black;
            inventory.slots[idx].transform.GetChild(0).GetComponent<Text>().text = "";
        }


        else 
        {
            inventory.slots[idx].sprite = (Sprite)temInfo[csvReader.itemHeaderPairs["icon"]];
            inventory.slots[idx].color = Color.white;


            if (temType == 1)
            {
                inventory.slots[idx].transform.GetChild(0).GetComponent<Text>().text = "";
            }

            else
            {
                inventory.slots[idx].transform.GetChild(0).GetComponent<Text>().text = 
                    inventory.itemCount[idx].ToString();
            }
        }

    }
    #endregion
}
