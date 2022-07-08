using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Equip : MonoBehaviour
{
    public List<int> equipIndexs = new List<int>();

    public List<Button> equipItemSlot = new List<Button>();
    public List<Text> equipText = new List<Text>();

    public Image hpFill;
    public Image mpFill;

    CanvasGroup _thisCG;
    bool _isopen = false;

    private void Awake()
    {
        _thisCG = GetComponent<CanvasGroup>();
        for (int i = 0; i < 3; i++) equipIndexs.Add(0);
    }

    private void Start()
    {
        initEquipItemSlots();
    }

    private void Update()
    {
        _isopen = (_thisCG.alpha == 1);
        updateInfo();
    }

    private void initEquipItemSlots()
    {
        Button weapon = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>();
        Button defend = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Button>();
        Button acc = transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<Button>();

        equipItemSlot.Add(weapon);
        equipItemSlot.Add(defend);
        equipItemSlot.Add(acc);

        // 텍스트 벨류 최상위 부모
        Transform _textTartget = transform.GetChild(2).GetChild(1).GetChild(1).transform;

        equipText = new List<Text>(_textTartget.GetComponentsInChildren<Text>());

        hpFill = _textTartget.GetChild(_textTartget.childCount - 2).GetChild(0).GetComponent<Image>();
        mpFill = _textTartget.GetChild(_textTartget.childCount - 1).GetChild(0).GetComponent<Image>();
    }

    private void updateInfo()
    {
        if (!_isopen) return;

        List<object> pData =  Data_Manager.instance.equip_response_player_data();

        int _hp_cur = (int)pData[pData.Count - 4];
        int _hp_max = (int)pData[pData.Count - 3];

        int _mp_cur = (int)pData[pData.Count - 2];
        int _mp_max = (int)pData[pData.Count - 1];

        // 닉네임
        equipText[0].text = (string)pData[0];

        // 레벨
        equipText[1].text = ((int)pData[1]).ToString();

        // 직업
        equipText[2].text = Data_Manager.instance.job_idx_to_string((int)pData[2]);

        // 체력
        equipText[3].text = _hp_max.ToString() + "/" + _hp_cur.ToString();

        // 마력
        equipText[4].text = _mp_max.ToString() + "/" + _mp_cur.ToString();

        // 힘
        equipText[5].text = ((int)pData[3]).ToString();

        // 민
        equipText[6].text = ((int)pData[4]).ToString();

        // 지
        equipText[7].text = ((int)pData[5]).ToString();

        // 공격력
        equipText[8].text = ((double)pData[6]).ToString();

        // 방어력
        equipText[9].text = ((int)pData[7]).ToString();

        // 잔여 스탯 포인트
        equipText[10].text = ((int)pData[8]).ToString();

        // 잔여 스킬 포인트
        equipText[11].text = ((int)pData[9]).ToString();

        // 체력 바 업데이트
        hpFill.fillAmount = (float)  _hp_cur / _hp_max;

        // 마나 바 업데이트
        mpFill.fillAmount = (float)_mp_cur / _mp_max;
    }
}
