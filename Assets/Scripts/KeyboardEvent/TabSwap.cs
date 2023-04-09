using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;


public class TabSwap : MonoBehaviour
{
    //public int InputSelected;
    public static TabSwap _tabSwap;
    public static Stack<TabList> stack_tabList;
    
    void Awake()
    {
        if (_tabSwap != null && _tabSwap != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _tabSwap = this;
        }
        stack_tabList = new Stack<TabList>();
    }

    void Update()
    {
        TabList tabListScript = stack_tabList.Peek();
        if (Input.GetKeyDown(KeyCode.Escape)) stack_tabList.Peek().GetComponent<PanelBase>().Exit();
        if (tabListScript.tabList_onlyInput.Count != 0 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            Button selectedBtn = null;
            if (!tabListScript.tabList[tabListScript.inputSelected].TryGetComponent<Button>(out selectedBtn) &&
                tabListScript.tabList[tabListScript.inputSelected] == tabListScript.tabList_onlyInput[tabListScript.tabList_onlyInput.Count - 1]&& !string.IsNullOrEmpty(tabListScript.tabList[tabListScript.inputSelected].GetComponent<TMP_InputField>().text))
            {
                stack_tabList.Peek().GetComponent<PanelBase>().Complete();
            }
        }

        if (stack_tabList.Peek().tabList.Count <= 1) return;

        if(Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            tabListScript.inputSelected--;
            if (tabListScript.inputSelected < 0) tabListScript.inputSelected = tabListScript.tabList.Count-1;
            SelectInputFieldByTab();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabListScript.inputSelected++;
            if (tabListScript.inputSelected >= tabListScript.tabList.Count) tabListScript.inputSelected = 0;
            SelectInputFieldByTab();
        }
    }
    public void SelectInputFieldByTab()
    {
        TabList tabListScript = stack_tabList.Peek();
        tabListScript.tabList[tabListScript.inputSelected].Select();
    }
    public void CheckTabList()
    {
        Debug.Log(stack_tabList.Peek());
    }
    public void CheckTablistPanel()
    {
        //디버깅용으로 버튼에 넣은 메서드
        Debug.Log(stack_tabList.Count);
    }
}
