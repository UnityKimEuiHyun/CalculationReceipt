using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_OverwriteFinalCheck : PanelBase
{
    [Header("UI_Text")]
    public TMP_Text txt_comment;

    [Header("UI_Button")]
    public Button btn_okay;
    public Button btn_no;

    public void OnInit(string saveName, int index, string newSaveName)
    {
        base.OnInit();
        txt_comment.text = $"���� ������[{saveName}]��\n���ο� ������[{newSaveName}]�� �����ðڽ��ϱ�";
        txt_comment.color = GameManager._gm.color_txt_caution;
        btn_okay.onClick.AddListener(() => Complete(index, newSaveName));
        btn_no.onClick.AddListener(Exit);
    }
    public void OnInit(string saveName, int index)
    {
        base.OnInit();
        txt_comment.text = $"���� ������[{saveName}]��\n���ο� �����͸� �����ðڽ��ϱ�";
        txt_comment.color = GameManager._gm.color_txt_caution;
        btn_okay.onClick.AddListener(() => Complete(index));
        btn_no.onClick.AddListener(Exit);
    }
    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
    public void Complete(int index)
    {
        base.Complete();
        ChangeData(index);
        Popup_SaveListOverwrite existedScript;
        if (TabSwap.stack_tabList.Peek().TryGetComponent<Popup_SaveListOverwrite>(out existedScript))
            existedScript.Exit();
        Destroy(backG);
    }
    public void Complete(int index, string newSaveName)
    {
        base.Complete();
        ChangeData(index, newSaveName);
        Popup_SaveListOverwrite existedScript;
        if(TabSwap.stack_tabList.Peek().TryGetComponent<Popup_SaveListOverwrite>(out existedScript))
            existedScript.Exit();
        Destroy(backG);
    }
}
