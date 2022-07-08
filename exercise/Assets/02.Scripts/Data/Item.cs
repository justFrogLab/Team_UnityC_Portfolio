using UnityEngine;

public class Item
{
    public int mainIndex;
    public int itemType;
    public string itemName;
    public string itemInfo;
    public int buyGold;
    public int sellGold;
    public Sprite icon;
}

public class Item_Equipment : Item
{
    public int reqJob;
    public int reqLevel;
    public int equipPart;
    public int statHp;
    public int statMp;
    public int statDmg;
}

public class Item_Consumable : Item
{
    public int HPUP;
    public int MPUP;
    public int duration;
    public int coolTime;
}

public class Item_Immediately : Item
{
    public int getExp;
    public int getGold;
}