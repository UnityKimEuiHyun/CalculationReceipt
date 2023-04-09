using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Popup_ResultofFindPW : PanelBase
{
    [Header("UI_Text")]
    public TMP_Text txt_result;
    [Header("UI_Button")]
    public Button btn_Okay;

    string id, pw;
    int pwLength;

    public void OnInit(string userId)
    {
        base.OnInit();
        id = userId;
        btn_Okay.onClick.AddListener(Exit);
        txt_result.text = $"검색하신 '{id}'는 가입되지 않은 계정입니다.";
    }
    public void OnInit(AccountInfo result)
    {
        base.OnInit();
        btn_Okay.onClick.AddListener(Complete);
        SetData(result);
    }
    void SetData(AccountInfo result)
    {
        id = result.id;
        pw = result.pw;
        pwLength = pw.Length;
        var safePw = pw.Remove(3, pwLength - 3);

        txt_result.text = $"검색하신 '{id}' 계정의 비밀번호는\n{safePw}로 시작하고 {pwLength} 글자로 구성된 암호입니다.";
    }

    public override void Complete()
    {
        base.Complete();
        GameManager._loginPanel.input_loginId.text = id;
        GameManager._panel_findPW.Exit();
        TabSwap.stack_tabList.Peek().SelectInputField(1);
        Destroy(backG);
    }
    public override void Exit()
    {
        base.Exit();
        TabSwap.stack_tabList.Peek().SelectInputField(0);
        Destroy(backG);
    }

}
