using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    [Header("UI_GameManager")]
    public static GameManager _gm;

    [Header("UI")]
    public static GameObject panels;
    public static Transform _panels;

    [Header("PanelScript")]
    public static LoginPanelScript _loginPanel;
    public static HomePanel _homePanel;
    public static NewAccountPanelScript _panel_newAccount;
    public static FindIDPanelScript _panel_findID;
    public static FindPWPanelScript _panel_findPW;
    public static Panel_CostMain _panel_costMain;
    public static Panel_SaveList _panel_saveList;

    [Header("UI 색상 관리")]
    public Color color_btn_normal;
    public Color color_btn_caution, color_btn_complete, color_btn_disable, color_txt_normal, color_txt_confirmed, color_txt_caution, color_txt_white, color_backG_A0, color_backG_A60;
    public Color color_btn_selected, color_btn_unselected;

    public static AccountInfo loginInfo;

    private void Awake()
    {
        if (_gm != null && _gm != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _gm = this;
        }
    }
    private void Start()
    {
        panels = GameObject.Find("Panels");
        _panels = panels.GetComponent<Transform>();
        _homePanel = null;
        _loginPanel = null;
        _panel_newAccount = null;
        _panel_findPW = null;
        _panel_findID = null;
        SetloginPanel();
    }
    
    public void SetloginPanel()
    {
        var asset = Resources.Load<GameObject>("Prefabs/Panels/panel_Login");
        var prefab = GameObject.Instantiate(asset, _panels);
        prefab.GetComponent<LoginPanelScript>().OnInit();
    }
    public void SetHomePanel()
    {
        var asset = Resources.Load<GameObject>("Prefabs/Panels/panel_Home");
        var prefab = GameObject.Instantiate(asset);
        prefab.GetComponent<HomePanel>().OnInit();
    }
    
}

