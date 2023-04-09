using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_DeleteAll : PanelBase
{
    [Header("UI_Text")]
    public TMP_Text txt_title;
    public TMP_Text txt_comment;
    [Header("UI_Button")]
    public Button btn_yes;
    public Button btn_no;
    [Header("UI_Content")]
    public Transform content;

    public void OnInit(string titleTxt, string commentTxt, Transform content)
    {
        base.OnInit();
        txt_title.text = titleTxt;
        txt_comment.text = commentTxt;
        txt_comment.color = GameManager._gm.color_txt_caution;
        this.content = content;
        btn_yes.onClick.AddListener(Complete);
        btn_no.onClick.AddListener(Exit);
    }
    public override void Complete()
    {
        base.Complete();
        ClearChildren(content);
        loginInfo.Data_saveList.Clear();
        GameManager._panel_saveList.SetList();
        OverWriteJson();
        Destroy(backG);
    }

    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
}
