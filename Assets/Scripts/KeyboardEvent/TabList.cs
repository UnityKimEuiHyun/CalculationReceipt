using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TabList : MonoBehaviour
{
    public List<Selectable> tabList;
    public List<TMP_InputField> tabList_onlyInput;
    public List<Button> tabList_onlyBtn;
    public int inputSelected = 0;

    
    public void SelectInputField(int num)
    {
        tabList[num].Select();
        inputSelected = num;
    }
    public void ChangeInputNumAndSelect(int num)
    {
        if (tabList.Count <= 1) return;
        inputSelected += num;
        if ( inputSelected < 0) inputSelected = tabList.Count - 1;
        tabList[inputSelected].Select();
    }
}
