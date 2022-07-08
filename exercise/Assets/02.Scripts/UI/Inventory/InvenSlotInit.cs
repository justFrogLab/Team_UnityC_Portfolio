using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlotInit : MonoBehaviour
{
    [SerializeField]
    GameObject slotPrefab;

    [SerializeField]
    RectTransform invenSlotsRT;

    string slotString = "invenItemSlot";

    [Space()]

    [SerializeField]
    GameObject slotBackgroundPrefab;

    [SerializeField]
    RectTransform slotBackRT;

    string slotBackString = "invenBack";

    float slotsize = 120f;
    float contentAreahori = 50f;
    float contentAreavert = -45f;
    int horizontalcount = 5;
    int vericalcount = 5;
    float slotMargin = 15f;


    void Awake()
    {
        InitSlots(slotPrefab, slotString, invenSlotsRT);
        InitSlots(slotBackgroundPrefab, slotBackString, slotBackRT);
    }

    void InitSlots(GameObject _prefab, string _name, RectTransform _rect)
    {
        _prefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(slotsize, slotsize);
        //_prefab.TryGetComponent(out SlotCtrl itemSlot);
        //if (itemSlot == null)
        //    _prefab.AddComponent<SlotCtrl>();
        _prefab.SetActive(false);
        Vector2 beginPos = new Vector2(contentAreahori, contentAreavert);
        Vector2 curPos = beginPos;

        //List<SlotCtrl> slotUIList = new List<SlotCtrl>(vericalcount * horizontalcount);
        for (int j = 0; j < vericalcount; j++)
        {
            for (int i = 0; i < horizontalcount; i++)
            {
                int slotindex = (horizontalcount * j) + i;
                var slotRT = CloneSlot();
                slotRT.pivot = new Vector2(0.5f, 0.5f);
                slotRT.anchoredPosition = curPos;
                slotRT.localScale = new Vector3(1f, 1f, 1f);
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = _name + $"@[{slotindex}]";

                //var slotUI = slotRT.GetComponent<SlotCtrl>();
                //slotUIList.Add(slotUI);
                curPos.x += (slotMargin + slotsize);
            }
            curPos.x = beginPos.x;
            curPos.y -= (slotMargin + slotsize);
        }
        if (_prefab.scene.rootCount != 0)
        {
            Destroy(_prefab);
        }
        RectTransform CloneSlot()
        {
            GameObject slotGo = Instantiate(_prefab);
            RectTransform rt = slotGo.GetComponent<RectTransform>();
            rt.SetParent(_rect);
            return rt;
        }
    }
}
