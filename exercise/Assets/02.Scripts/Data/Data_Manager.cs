using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Manager : MonoBehaviour
{
    public static Data_Manager instance;

    [SerializeField] playerCtrl _playerCtrl;
    [SerializeField] Inventory inventory;
    [SerializeField] Equip equip;

    #region 정보에 따른 매칭 값이 필요합니다.
    Dictionary<int, string> job_dic = new Dictionary<int, string>();
    Dictionary<int, string> item_part_dic = new Dictionary<int, string>();
    #endregion

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _initDic();
    }

    void _initDic()
    {
        job_dic.Add(1, "견습 기사");
        job_dic.Add(2, "전사");
        job_dic.Add(3, "총사");

        item_part_dic.Add(1, "무기구");
        item_part_dic.Add(2, "방어구");
        item_part_dic.Add(3, "장신구");
    }

    #region dictnory 반환
    public string job_idx_to_string(int idx)
    {
        return job_dic[idx];

    }
    #endregion

    #region invenslotManager ::: 실제 있는 값인지 검증 부탁드리겠습니다.
    public int invenValueNull(int idx)
    {
        // 인벤토리에서 해당 아이템 값이 없으면 안되겟죠
        int rValue = (inventory.itemIndexs[idx] == 0 || inventory.itemCount[idx] == 0) ? -1 : idx;

        return rValue;
    }
    #endregion

    #region invenslotManager ::: 실제 있는 값인지 검증 부탁드리겠습니다.
    public int equipValueNull(int idx)
    {
        // 인벤토리에서 해당 아이템 값이 없으면 안되겟죠
        int rValue = (equip.equipIndexs[idx] == 0) ? -1 : idx;

        return rValue;
    }
    #endregion


    #region invenslotManager ::: 해당 순서에 해당하는 값을 반환 부탁 드릴게요.
    public int get_inven_Index(int idx)
    {
        return inventory.itemIndexs[idx];
    }
    #endregion

    #region invenslotManager ::: 해당 순서에 해당하는 값을 반환 부탁 드릴게요.
    public int get_equip_Index(int idx)
    {
        return equip.equipIndexs[idx];
    }
    #endregion

    #region invenslotManager ::: 포션 아이템을 사용 했어요!
    public void useItem(int idx)
    {
        // 현재 남아 있는 아이템 갯수 입니다.
        int remainCount = inventory.itemCount[idx];

        // 마지막 아이템 인가요?
        bool last = (--remainCount <= 0);

        // 마지막 아이템이 아니라면 카운트만 내려 줍니다.
        if (!last) inventory.itemCount[idx]--;

        // 마지막 아이템 이라면 해당 인덱스도 제거 합니다.
        else
        {
            inventory.itemCount[idx] = 0;
            inventory.itemIndexs[idx] = 0;
        }
    }
    #endregion

    #region invenslotManager ::: 포션 사용에 따른 유저의 체력, 마나를 갱신합니다
    public void user_hill_from_itemPotion(List<object> itemInfo)
    {
        int hill_hp = (int)itemInfo[csvReader.itemHeaderPairs["HPUP"]];
        int hill_mp = (int)itemInfo[csvReader.itemHeaderPairs["MPUP"]];

        _playerCtrl.hpCur = (int)Mathf.Clamp(_playerCtrl.hpCur + hill_hp, 0f, _playerCtrl.hpMax);
        _playerCtrl.mpCur = (int)Mathf.Clamp(_playerCtrl.mpCur + hill_mp, 0f, _playerCtrl.mpMax);
    }
    #endregion

    #region invenslotManager ::: 장착 중인 아이템이 있을까요?
    public int have_equipPart(int part)
    {
        return (equip.equipIndexs[part - 1] != 0) ? equip.equipIndexs[part - 1] : -1;
    }
    #endregion

    #region invenslotManager ::: 아이템을 장비에 전송합니다.
    public void mountEquip(int idx, int part)
    {
        equip.equipIndexs[part - 1] = idx;
    }
    #endregion

    #region invenslotManager ::: 무기를 인벤토리에 전송
    public void equip_to_inven_item(int idx, int itemIdx)
    {
        inventory.itemIndexs[idx] = itemIdx;
        inventory.itemCount[idx] = 1;
    }
    #endregion

    #region invenslotManager ::: 무기를 장비에서 삭제
    public void equip_remove_idx(int idx)
    {
        equip.equipIndexs[idx] = 0;
    }
    #endregion
    #region Equip ::: 플레이어 정보 전체를 주세요.
    public List<object> equip_response_player_data()
    {
        List<object> _data = new List<object>();
        _data.Add(_playerCtrl.name);
        _data.Add(_playerCtrl.lv);
        _data.Add(_playerCtrl.job);
        _data.Add(_playerCtrl.str);
        _data.Add(_playerCtrl.dex);
        _data.Add(_playerCtrl.inter);
        _data.Add(_playerCtrl.damage);
        _data.Add(_playerCtrl.def);
        _data.Add(_playerCtrl.statP);
        _data.Add(_playerCtrl.skillP);

        _data.Add(_playerCtrl.hpCur);
        _data.Add(_playerCtrl.hpMax);

        _data.Add(_playerCtrl.mpCur);
        _data.Add(_playerCtrl.mpMax);

        return _data;
    }
    #endregion

    #region 인벤토리 반칸 확인용도
    public int invnen_to_empty()
    {
        return inventory.checkEmpty();
    }
    #endregion

    #region 인벤토리 골드 업데이트
    public int gold_info_get()
    {
        return _playerCtrl.gold;
    }
    #endregion

    #region dropitem ::: 해당 인벤토리에 몇 개 있는지?
    public int inven_item_count(int idx)
    {
        return inventory.itemCount[idx];
    }
    #endregion

    #region 플레이어 디스플레이 업데이트
    public List<int> get_display_charcter_info()
    {
        List<int> _data = new List<int>();

        _data.Add(_playerCtrl.hpCur);
        _data.Add(_playerCtrl.hpMax);
        _data.Add(_playerCtrl.mpCur);
        _data.Add(_playerCtrl.mpMax);

        return _data;
    }
    #endregion

    #region 인벤토리 안에서 변경
    public void exchangeInvenItemSlot(int org_idx, int target_idx)
    {
        //
        // 임시 정보
        int tempMainIdx = inventory.itemIndexs[org_idx];
        int tempCount = inventory.itemCount[org_idx];

        // 타겟 정보 -> 오리진 정보
        inventory.itemIndexs[org_idx] = inventory.itemIndexs[target_idx];
        inventory.itemCount[org_idx] = inventory.itemCount[target_idx];

        // 임시 정보 -> 타겟 정보
        inventory.itemIndexs[target_idx] = tempMainIdx;
        inventory.itemCount[target_idx] = tempCount;
    }
    #endregion
}
