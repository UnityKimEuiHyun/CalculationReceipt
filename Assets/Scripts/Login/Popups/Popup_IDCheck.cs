using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Popup_IDCheck : PanelBase
{
    [Header("UI_Button")]
    public Button btn_Okay;
    public Button btn_No;
    [Header("UI_Text")]
    public TMP_Text txt_idCheckTxt;

    
    public override void OnInit()
    {
        base.OnInit();
        txt_idCheckTxt.text
            = $"{GameManager._panel_newAccount.input_id.text}는 사용 가능한 아이디 입니다.";
        btn_Okay.onClick.RemoveAllListeners();
        btn_Okay.onClick.AddListener(Complete);
        btn_No.onClick.RemoveAllListeners();
        btn_No.onClick.AddListener(Exit);
    }
    public override void Exit()
    {
        base.Exit();
        GameManager._panel_newAccount.txt_idComment.text = "ID 입력 후 중복확인을 진행해주세요.";
        GameManager._panel_newAccount.FinishID = false;
        TabSwap.stack_tabList.Peek().SelectInputField(0);
        Destroy(backG);
    }
    public override void Complete()
    {
        base.Complete();
        GameManager._panel_newAccount.txt_idComment.text = "ID is Confirmed!";
        GameManager._panel_newAccount.txt_idComment.color = GameManager._gm.color_txt_confirmed;
        GameManager._panel_newAccount.btn_idCheck.GetComponent<Image>().color = GameManager._gm.color_btn_complete;
        GameManager._panel_newAccount.FinishID = true;
        TabSwap.stack_tabList.Peek().SelectInputField(2);
        Destroy(backG);
    }
}
