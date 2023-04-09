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
        txt_telCheckComment.text = "�ӽ÷� ������ȣ�� 1234�� �����صξ����ϴ�.";
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
            txt_telCheckComment.text = "�ð��� �ʰ��Ǿ����ϴ�. �ٽ� �õ����ּ���.";
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
            txt_telCheckComment.text = "������ȣ�� ��ġ���� �ʽ��ϴ�. �ٽ��ѹ� Ȯ�����ּ���.";
            txt_telCheckComment.color = GameManager._gm.color_txt_caution;
        }
        // ���ں��� ��ȣ�� �������� Ȯ��
        // �����ϸ� �����Ϸ�, �������� ������ Comment
        // text "��ġ��������" ���� 
    }
}
