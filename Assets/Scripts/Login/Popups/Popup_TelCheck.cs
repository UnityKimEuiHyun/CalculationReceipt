using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_TelCheck : PanelBase
{
    [Header("UI_Button")]
    public Button btn_Okay;
    public Button btn_No;
    [Header("UI_Inputfield")]
    public TMP_InputField input_checkNum;
    [Header("UI_Text")]
    public TMP_Text txt_telCheckComment;
    public TMP_Text txt_timerNum;
    float timer_sec;
    public bool IsOkay;
    public bool doTimer = false;
    public override void OnInit()
    {
        base.OnInit();
        timer_sec = 300;
        btn_Okay.onClick.RemoveAllListeners();
        btn_Okay.onClick.AddListener(Complete);
        btn_No.onClick.RemoveAllListeners();
        btn_No.onClick.AddListener(Exit);
        txt_telCheckComment.text = "임시로 인증번호는 1234로 설정해두었습니다.";
        txt_telCheckComment.color = GameManager._gm.color_txt_normal;
        input_checkNum.text = null;
        input_checkNum.Select();
    }
    public void Update()
    {
        if (timer_sec > 0)
        {
            timer_sec -= Time.deltaTime;
            txt_timerNum.text = $"{(int)timer_sec} / 300 sec";
            txt_timerNum.color = GameManager._gm.color_txt_normal;
            IsOkay = true;
            input_checkNum.interactable = true;
            btn_Okay.interactable = true;
        }
        else
        {
            txt_timerNum.text = $"0 / 300 sec";
            txt_telCheckComment.text = "시간이 초과되었습니다. 다시 시도해주세요.";
            txt_timerNum.color = GameManager._gm.color_txt_caution;
            txt_telCheckComment.color = GameManager._gm.color_txt_caution;
            IsOkay = false;
            input_checkNum.interactable = false;
            btn_Okay.interactable = false;
        }
    }
        

    public override void Exit()
    {
        base.Exit();
        TabSwap.stack_tabList.Peek().SelectInputField(5);
        GameManager._panel_newAccount.FinishTEL = false;
        Destroy(backG);
    }
    public override void Complete()
    {
        if(input_checkNum.text == GameManager._panel_newAccount.telCode)
        {
            base.Complete();
            GameManager._panel_newAccount.FinishTEL = true;
            TabSwap.stack_tabList.Peek().SelectInputField(7);
            Destroy(backG);
        }
        else
        {
            txt_telCheckComment.text = "인증번호와 일치하지 않습니다. 다시한번 확인해주세요.";
            txt_telCheckComment.color = GameManager._gm.color_txt_caution;
        }
        // 문자보낸 번호와 동일한지 확인
        // 동일하면 인증완료, 동일하지 않으면 Comment
        // text "일치하지않음" 변경 
    }
}
