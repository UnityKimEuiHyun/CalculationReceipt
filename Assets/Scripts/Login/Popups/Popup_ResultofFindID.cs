using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;


public class Popup_ResultofFindID : PanelBase
{
    [Header("UI_Text")]
    public TMP_Text txt_resultComment;
    [Header("UI_ScrollView_Content")]
    public Transform result_content;
    [Header("UI_Button")]
    public Button btn_Okay;

    public void OnInit(string userName, List<AccountInfo> datas)
    {
        base.OnInit();
        Result_FindID(datas);
        txt_resultComment.text = $"검색된 {userName}님의 아이디는 다음과 같습니다.";
        btn_Okay.onClick.AddListener(Exit);
    }
    public override void OnInit()
    {
        base.OnInit();
        txt_resultComment.text = "일치하는 계정이 없습니다.";
        txt_resultComment.color = GameManager._gm.color_txt_caution;
        btn_Okay.onClick.AddListener(Exit);
    }

    void Result_FindID(List<AccountInfo> datas)
    {
        if (datas.Count == 0) return;
        GetComponent<TabList>().tabList.Clear();
        for (int i = 0; i < datas.Count; i++)
        {
            var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_resultOfFindID");
            var prefab = GameObject.Instantiate(asset, result_content);
            var btnScript = prefab.GetComponent<Result_FindIdScript>();
            btnScript.txt_userId.text = datas[i].id;
            btnScript.txt_regiDate.text = datas[i].registerDate;
            btnScript.btn_Id.onClick.AddListener(() => Btn_FindIDResultPrefab(btnScript));
            GetComponent<TabList>().tabList.Add(btnScript.btn_Id);
        }
        GetComponent<TabList>().tabList.Add(btn_Okay);
        TabSwap.stack_tabList.Peek().SelectInputField(0);
    }
    public override void Complete()
    {
        Exit();
    }
    public override void Exit()
    {
        base.Exit();
        TabSwap.stack_tabList.Peek().SelectInputField(0);
        Destroy(backG);
    }

    void Btn_FindIDResultPrefab(Result_FindIdScript btnScript)
    {
        GameManager._loginPanel.input_loginId.text = btnScript.txt_userId.text;
        base.Exit();
        GameManager._panel_findID.Exit();
        TabSwap.stack_tabList.Peek().SelectInputField(1);
        Destroy(backG);
    }
}
