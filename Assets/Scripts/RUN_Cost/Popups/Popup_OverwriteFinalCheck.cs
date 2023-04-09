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
        txt_comment.text = $"기존 데이터[{saveName}]에\n새로운 데이터[{newSaveName}]를 덮어씌우시겠습니까";
        txt_comment.color = GameManager._gm.color_txt_caution;
        btn_okay.onClick.AddListener(() => Complete(index, newSaveName));
        btn_no.onClick.AddListener(Exit);
    }
    public void OnInit(string saveName, int index)
    {
        base.OnInit();
        txt_comment.text = $"기존 데이터[{saveName}]에\n새로운 데이터를 덮어씌우시겠습니까";
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
