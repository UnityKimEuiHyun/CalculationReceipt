using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : PanelBase
{
    [Header("UI_Button")]
    public Button btn_CostPanel;
    public Button btn_logOut;

    public override void OnInit()
    {
        base.OnInit();
        GameManager._homePanel = this;
        btn_CostPanel.onClick.AddListener(() => Set_CostMain());
        btn_logOut.onClick.AddListener(Exit);
    }
    public void Set_CostMain()
    {
        base.Exit();
        var asset_costMain = Resources.Load<GameObject>("Prefabs/Panels/panel_CostMain");
        var popup_costMain = CreatePanel(asset_costMain);
        popup_costMain.GetComponent<Panel_CostMain>().OnInit();
        Destroy(backG);
    }
    public override void Exit()
    {
        base.Exit();
        GameManager.loginInfo = null;
        TabSwap.stack_tabList.Peek().tabList_onlyInput[0].Select();
        Destroy(backG);
    }
}
