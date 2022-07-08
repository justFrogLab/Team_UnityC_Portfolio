using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerDisplayInfo : MonoBehaviour
{
    [SerializeField] Text hp_txt;
    [SerializeField] Text mp_txt;
    [SerializeField] Image hp_bar;
    [SerializeField] Image mp_bar;

    void Update()
    {
        update_display_character_info();
    }

    void update_display_character_info()
    {
        List<int> _data = Data_Manager.instance.get_display_charcter_info();

        hp_txt.text = ((int)_data[1]).ToString() + "/" + ((int)_data[0]).ToString();
        mp_txt.text = ((int)_data[3]).ToString() + "/" + ((int)_data[2]).ToString();

        hp_bar.fillAmount = (float)_data[0] / _data[1];
        mp_bar.fillAmount = (float)_data[2] / _data[3];
    }
}