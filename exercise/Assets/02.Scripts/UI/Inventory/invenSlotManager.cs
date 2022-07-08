using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class invenSlotManager : MonoBehaviour,IBeginDragHandler, IDragHandler, IDropHandler
{
    #region 그래픽 레이 캐스트
    GraphicRaycaster GR;
    List<RaycastResult> rayResults;
    PointerEventData PED;
    #endregion

    #region 커서가 인벤토리 슬롯에 올라와 있나요?
    readonly string _inItem = "invenItemSlot";
    readonly string _equip_item = "equipItem";

    bool _inven_open_click = false;
    bool _open_info_window = false;
    bool _equip_open_click = false;
    #endregion

    #region 슬롯이 맞다면 보여줄 정보가 존재해요.
    [SerializeField] CanvasGroup _infoItemCG;
    List<Text> _slot = new List<Text>();
    #endregion

    #region 인벤토리에서 장비를 장착했다면 장비창도 필요해요!
    [SerializeField] CanvasGroup equipCG;
    #endregion

    #region 정보에 따른 매칭 값이 필요합니다.
    Dictionary<int, string> req_job_dic = new Dictionary<int, string>();
    Dictionary<int, string> item_part_dic = new Dictionary<int, string>();
    #endregion

    #region 골드 업데이트
    [SerializeField] Text invenGold;
    #endregion

    #region 인벤토리 rectRANFORM
    [SerializeField] RectTransform inventoryRectransform;
    #endregion

    #region 버릴 아이템 정보
    [SerializeField] CanvasGroup itemDropCG;
    #endregion

    private void Awake()
    {
        GR = GetComponent<GraphicRaycaster>();

    }

    void Start()
    {
        _initItemCGinfo();
        _initDic();
    }

    void _initDic()
    {
        req_job_dic.Add(1, "공통");
        req_job_dic.Add(2, "전사");
        req_job_dic.Add(3, "총사");

        item_part_dic.Add(1, "무기구");
        item_part_dic.Add(2, "방어구");
        item_part_dic.Add(3, "장신구");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray(); // 정보를 갱신 합시다!
            _showInfo();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _ray(); // 정보를 갱신 합시다!
            _readItemInfo();
        }

        inven_gold_update();
    }

    #region 골드 업데이트
    public void inven_gold_update()
    {
        int _gold = Data_Manager.instance.gold_info_get();
        //Debug.Log(string.Format("{0:#,###}", _gold));
        invenGold.text = string.Format("{0:#,###}", _gold).ToString();
    }
    #endregion

    #region 좌클릭
    #region UI 클릭 ::: 정보 습득
    public void _ray()
    {
        rayResults = new List<RaycastResult>();     // 초기화
        PED = new PointerEventData(null);       // 초기화

        PED.position = Input.mousePosition;     // 마우스 위치 정보
        GR.Raycast(PED, rayResults);        // 해당 하는 곳으로 레이 캐스트

        if (rayResults.Count <= 0)
        {
            _inven_open_click = false;    // ui는 확실히 아니네요.
            return;      // 결과 값이 없으면 return
        }

        string _objectName = rayResults[0].gameObject.transform.gameObject.name;    // 해당 하는 이름이
        string _oracle = _objectName.Split('@')[0];

        _inven_open_click = _oracle == _inItem;     // 인벤 슬롯일까요?
        _equip_open_click = _oracle == _equip_item;   // 장비 슬롯일까요?
    }
    #endregion

    #region 아이템 정보를 보여 줍시다~
    public void _showInfo()
    {
        if (!_inven_open_click)
        {
            _open_info_window = true;
        }

        _open_info_window = _infoItemCG.alpha == 1; // 이미 아이템 정보창이 열려 있나요?

        if (_open_info_window) UI_Manager.instance.pop_stack(_infoItemCG);

        // 빈 값이 아닌 경우에만 작동!
        if (!_open_info_window && !_checkNull() && (_inven_open_click || _equip_open_click))
        {
            // 위치 이동
            _infoItemCG.GetComponent<RectTransform>().anchoredPosition
                = new Vector3(
                rayResults[0].screenPosition.x,
                rayResults[0].screenPosition.y);

            // 아이템 순서를 읽어 올게요
            int thisIdx = rayResults[0].gameObject.transform.GetSiblingIndex();

            if (_inven_open_click)    // 인벤토리 슬롯 아이템
            {
                // 데이터 매니저에서 해당 인덱스를 가져 옵시다!!
                int itemIdx = Data_Manager.instance.get_inven_Index(thisIdx);

                // csv에서 해당 아이템의 정보 리스트를 저장합니다.
                List<object> thisInfo = csvReader.itemData[csvReader.itemIndexPairs[itemIdx]];

                int itemType = (int)thisInfo[csvReader.itemHeaderPairs["itemType"]];

                if (itemType == 1)
                {
                    _slot[0].text = "□ 이름 : " + (string)thisInfo[csvReader.itemHeaderPairs["itemName"]];

                    _slot[1].text = "□ 요구 레벨 : " + ((int)thisInfo[csvReader.itemHeaderPairs["reqLevel"]]).ToString() + '\n' +
                                           "□ 요구 직업 : " + req_job_dic[(int)thisInfo[csvReader.itemHeaderPairs["reqJob"]]];

                    _slot[2].text = "□ 부위 : " + item_part_dic[(int)thisInfo[csvReader.itemHeaderPairs["equipPart"]]] + '\n' +
                                           "□ 체력 : " + ((int)thisInfo[csvReader.itemHeaderPairs["statHp"]]).ToString() + '\n' +
                                           "□ 마력 : " + ((int)thisInfo[csvReader.itemHeaderPairs["statMp"]]).ToString() + '\n' +
                                           "□ 공격 : " + ((int)thisInfo[csvReader.itemHeaderPairs["statDmg"]]).ToString();

                    _slot[3].text = "□ 설명 : " + (string)thisInfo[csvReader.itemHeaderPairs["itemInfo"]];
                }

                else if (itemType == 2)
                {
                    _slot[0].text = "□ 이름 : " + (string)thisInfo[csvReader.itemHeaderPairs["itemName"]];
                    _slot[1].text = "□ 체력 회복 : " + (int)thisInfo[csvReader.itemHeaderPairs["HPUP"]];
                    _slot[2].text = "□ 마력 회복 : " + (int)thisInfo[csvReader.itemHeaderPairs["MPUP"]];
                    _slot[3].text = "□ 설명 : " + (string)thisInfo[csvReader.itemHeaderPairs["itemInfo"]];


                }

                else
                {
                    _slot[0].text = "";
                    _slot[1].text = "";
                    _slot[2].text = "";
                    _slot[3].text = "";
                }
                //
                /*
                 
                 타입에 따라 설명 문구 다르게 하기
                 
                 */
            }

            else if (_equip_open_click)   // 장비 슬롯 아이템
            {
                // 데이터 매니저에서 해당 인덱스를 가져 옵시다!!
                int itemIdx = Data_Manager.instance.get_equip_Index(thisIdx);

                // csv에서 해당 아이템의 정보 리스트를 저장합니다.
                List<object> thisInfo = csvReader.itemData[csvReader.itemIndexPairs[itemIdx]];

                _slot[0].text = "□ 이름 : " + (string)thisInfo[csvReader.itemHeaderPairs["itemName"]];

                _slot[1].text = "□ 요구 레벨 : " + ((int)thisInfo[csvReader.itemHeaderPairs["reqLevel"]]).ToString() + '\n' +
                                       "□ 요구 직업 : " + req_job_dic[(int)thisInfo[csvReader.itemHeaderPairs["reqJob"]]];

                _slot[2].text = "□ 부위 : " + item_part_dic[(int)thisInfo[csvReader.itemHeaderPairs["equipPart"]]] + '\n' +
                                       "□ 체력 : " + ((int)thisInfo[csvReader.itemHeaderPairs["statHp"]]).ToString() + '\n' +
                                       "□ 마력 : " + ((int)thisInfo[csvReader.itemHeaderPairs["statMp"]]).ToString() + '\n' +
                                       "□ 공격 : " + ((int)thisInfo[csvReader.itemHeaderPairs["statDmg"]]).ToString();

                _slot[3].text = "□ 설명 : " + (string)thisInfo[csvReader.itemHeaderPairs["itemInfo"]];
            }

            UI_Manager.instance.push_stack(_infoItemCG);
        }

        else UI_Manager.instance.pop_stack(_infoItemCG);
    }
    #endregion

    #region 아이템 정보창을 전부 로드 해볼까요?
    public void _initItemCGinfo()
    {
        Transform parentTr = _infoItemCG.transform;

        Text _title = parentTr.GetChild(0).GetChild(0).GetComponent<Text>();
        Text _require = parentTr.GetChild(1).GetChild(0).GetComponent<Text>();
        Text _value = parentTr.GetChild(2).GetChild(0).GetComponent<Text>();
        Text _info = parentTr.GetChild(3).GetChild(0).GetComponent<Text>();

        _slot.Add(_title);
        _slot.Add(_require);
        _slot.Add(_value);
        _slot.Add(_info);
    }
    #endregion

    #endregion

    #region 공통
    #region 해당 인벤토리가 비어 있으면 어떠한 상호 작용도 이뤄져선 안됩니다.
    public bool _checkNull()
    {
        // 데이터 매니저가 이를 확인 시켜줘 겠죠?
        int value;
        if (_inven_open_click)
            value = Data_Manager.instance.invenValueNull(rayResults[0].gameObject.transform.GetSiblingIndex());
        else if (_equip_open_click)
            value = Data_Manager.instance.equipValueNull(rayResults[0].gameObject.transform.GetSiblingIndex());
        else
            value = -1;

        return (value == -1);
    }
    #endregion


    #endregion

    #region 우클릭
    public void _readItemInfo()
    {
        // 아이템 순서를 읽어 올게요
        int thisIdx = rayResults[0].gameObject.transform.GetSiblingIndex();

        // 아이템 설명창 끄기
        UI_Manager.instance.pop_stack(_infoItemCG);

        if (_inven_open_click)
        {
            #region 111
            // 데이터 매니저에서 해당 인덱스를 가져 옵시다!!
            int itemIdx = Data_Manager.instance.get_inven_Index(thisIdx);

            // csv에서 해당 아이템의 정보 리스트를 저장합니다.
            List<object> thisInfo = csvReader.itemData[csvReader.itemIndexPairs[itemIdx]];

            // 해당하는 아이템의 종류를 검사합니다.
            int type = (int)thisInfo[csvReader.itemHeaderPairs["itemType"]];

            // 장비 아이템 이라면? 장착이 되어야 겠죠!
            if (type == 1)
            {
                // 장착 아이템이라면 어디 파트 아이템인지 확인합니다.
                int equipPart = (int)thisInfo[csvReader.itemHeaderPairs["equipPart"]];

                // 우선 장착 아이템이 기존에 있는지 검사합니다.
                int havePart = Data_Manager.instance.have_equipPart(equipPart);

                // 아이템 설명 창은 꺼줍시다
                UI_Manager.instance.pop_stack(_infoItemCG);

                // 장착 중인 아이템이 존재하지 않아요!
                if (havePart == -1)
                {
                    // 데이터 매니저로 해당 인덱스를 전송해 줍시다.
                    Data_Manager.instance.mountEquip(itemIdx, equipPart);

                    // 장비 아이템을 전송 했으니 인벤토리 데이터를 삭제 합니다..
                    Data_Manager.instance.useItem(thisIdx);

                    // 인벤토리 내에 아이템시 사용되었네요 ui를 갱신합니다.
                    UI_Manager.instance.use_item(thisIdx);

                    // 장비 아이템 칸에서 보이게 갱신 합니다. ui 매니저
                    UI_Manager.instance.mount_update_item(equipPart, thisInfo);

                    // 장비 아이템 장착을 했네요? 장비 아이템 창도 보여 줍시다!
                    UI_Manager.instance.push_stack(equipCG);
                }

                // 장착 중인 아이템이 존재하네요...
                else
                {
                    // 데이터 매니저로 해당 인덱스를 전송해 줍시다.
                    Data_Manager.instance.mountEquip(itemIdx, equipPart);

                    // 장비 아이템을 전송 했으니 인벤토리 데이터를 삭제 합니다..
                    Data_Manager.instance.useItem(thisIdx);

                    // 이미 데이터는 삭제가 되었으니 해당했던 인벤토리에 아이템을 삽입합니다.
                    Data_Manager.instance.equip_to_inven_item(thisIdx, havePart);

                    // 장비 아이템 칸에서 보이게 갱신 합니다. ui 매니저
                    UI_Manager.instance.mount_update_item(equipPart, thisInfo);

                    // 값이 변동 되었으니 해당하는 인벤토리 ui도 갱신 해줍니다! ::: ui 매니저
                    UI_Manager.instance.inven_update_click_equip(thisIdx, thisInfo);
                }
            }

            // 소비 아이템 이라면? 사용이 되어야겠죠~ ::: 데이터 매니저
            else if (type == 2)
            {
                // 몇 번째 아이템인지? 현재 담긴 정보가 무엇인지? 아이템 사용!
                Data_Manager.instance.useItem(thisIdx);

                // 값이 변동 되었으니 해당하는 인벤토리 ui도 갱신 해줍니다! ::: ui 매니저
                UI_Manager.instance.use_item(thisIdx);

                // 유저의 체력, 마나의 값도 회복시켜줍니다 ::: 데이터 매니저
                Data_Manager.instance.user_hill_from_itemPotion(thisInfo);
            }
            #endregion
        }

        else if (_equip_open_click)
        {
            // 빈칸 체크
            int _targetIdx = Data_Manager.instance.invnen_to_empty();

            // 빈칸 없넹
            if (_targetIdx == -1)
            {
                Debug.Log("아이템 창을 비워주세요");
                return;
            }

            // 가지고 있는 인덱스
            int itemIdx = Data_Manager.instance.get_equip_Index(thisIdx);


            // csv에서 해당 아이템의 정보 리스트를 저장합니다.
            List<object> thisInfo = csvReader.itemData[csvReader.itemIndexPairs[itemIdx]];

            // 인벤토리로 전송
            Data_Manager.instance.equip_to_inven_item(_targetIdx, itemIdx);

            // 장비에서 데이터 삭제
            Data_Manager.instance.equip_remove_idx(thisIdx);

            // ui 업데이트
            UI_Manager.instance.inven_update_click_equip(_targetIdx, thisInfo);

            UI_Manager.instance.equip_update_click_equip(thisIdx);
        }
    }



    #endregion


    #region 마우스 움직임 제어부

    #region 아이템 슬롯 시작, 이동 값
    Vector2 _begin_mouse_point;
    Vector2 _move_mouse_point;
    Vector2 _end_mouse_point;
    #endregion

    #region 아이템 슬롯 본래 부모, 임시 부모
    Transform _parent_parent;
    Transform _parent;
    #endregion

    #region 기존 인덱스 보관
    int _idx_from_parent;
    #endregion



    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_inven_open_click) return;

        _begin_mouse_point = rayResults[0].gameObject.transform.position;
        _move_mouse_point = eventData.position;

        _parent_parent = rayResults[0].gameObject.transform.parent.parent.transform;
        _parent = rayResults[0].gameObject.transform.parent.transform;

        _idx_from_parent = rayResults[0].gameObject.transform.GetSiblingIndex();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_inven_open_click) return;

        // 선택한 창으로 끌어오기
        rayResults[0].gameObject.transform.position =
            _begin_mouse_point + (eventData.position - _move_mouse_point);

        // 정보창 꺼줌
        UI_Manager.instance.pop_stack(_infoItemCG);

        // 임시로 꺼내줌
        rayResults[0].gameObject.transform.SetParent(_parent_parent);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!_inven_open_click) return;

        // 값 저장
        Transform slot_out_rayTr = rayResults[0].gameObject.transform;

        // 마지막 포지션 위치 기억
        _end_mouse_point = slot_out_rayTr.localPosition;

        // 범위 밖을 계산해보자
        bool inven_out = inven_slot_out(_end_mouse_point);

        // 부모 원래대로 돌려 놓기
        slot_out_rayTr.SetParent(_parent);

        // 원래 순서대로 돌리기
        slot_out_rayTr.SetSiblingIndex(_idx_from_parent);

        // 원래 포지션
        slot_out_rayTr.position = _begin_mouse_point;

        if (!inven_out)
        {
            #region 범위 안
            // 새로운 레이 :: 인벤 안에서만 동작
            _ray();
            Transform slot_in_rayTr = rayResults[0].gameObject.transform;

            // 바뀔 슬롯 인덱스
            int target_slot_idx = slot_in_rayTr.GetSiblingIndex();
            #endregion
        }

        if (inven_out)
        {
            #region 범위 밖
            // ui 띄우기 :: 인벤토리 제어는 아이템 드롭 측에서 관리
            UI_Manager.instance.push_stack(itemDropCG);

            // idx 값 전송
            itemDropCG.GetComponent<dropItem>().idx = slot_out_rayTr.GetSiblingIndex();
            #endregion
        }

    }

    public bool inven_slot_out(Vector2 _end_mouse_point)
    {
        return (_end_mouse_point.x < inventoryRectransform.rect.xMin
            || _end_mouse_point.x > inventoryRectransform.rect.xMax
            || _end_mouse_point.y < inventoryRectransform.rect.yMin
            || _end_mouse_point.y > inventoryRectransform.rect.yMax);

    }
    #endregion
}
