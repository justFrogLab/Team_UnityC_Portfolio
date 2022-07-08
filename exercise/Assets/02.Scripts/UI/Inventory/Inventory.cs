using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{

    [SerializeField] Transform slotParent;



    int invenLength;
    public List<int> itemIndexs = new List<int>();
    public List<int> itemCount = new List<int>();

    public Image[] slots;


    private void Start()
    {
        slots = slotParent.GetComponentsInChildren<Image>();
        invenLength = slots.Length;

        for (int i = 0; i < invenLength; i++)
        {
            itemIndexs.Add(0);
            itemCount.Add(0);
        }


        itemIndexs[0] = 5010;
        itemCount[0] = 1;
        updateInvenUI();
    }


    public void updateInvenUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            int slotData = itemIndexs[i];
            int slotCount = itemCount[i];

            if (slotData != 0)
            {
                slots[i].sprite = (Sprite)csvReader.itemData[csvReader.itemIndexPairs[slotData]][csvReader.itemHeaderPairs["icon"]];
                slots[i].color = Color.white;
            }

            else
            {
                slots[i].sprite = null;
                slots[i].color = Color.black;
            }

            if (slotCount != 0)
            {
                if ((int)csvReader.itemData[csvReader.itemIndexPairs[slotData]][csvReader.itemHeaderPairs["itemType"]] != 1)
                slots[i].transform.GetChild(0).GetComponent<Text>().text = itemCount[i].ToString();
            }

            else
            {
                slots[i].transform.GetChild(0).GetComponent<Text>().text = null;
            }

        }
    }

    public int indexCheck(int itemIndex)
    {
        for (int i = 0; i < itemIndexs.Count; i++)   // 동일한 아이템이 있는지 검사
        {
            if (itemIndexs[i] == itemIndex)     // 동일한 아이템의 위치에 가서 있는지 검사
            {
                return i;

            }
        }
        return -1;
    }

    public int countEmpty()
    {
        int count = 0;
        for (int i = 0; i < invenLength; i++)
        {
            if (itemIndexs[i] == 0) count++;
        }
        return count;
    }

    public int checkEmpty()
    {
        for (int i = 0; i < invenLength; i++)
        {
            if (itemIndexs[i] == 0) return i;
        }
        return -1;
    }
}
