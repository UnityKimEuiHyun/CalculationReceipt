using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_LoadCaution : PanelBase
{
    [Header("UI_Text")]
    public TMP_Text txt_comment;
    [Header("UI_Button")]
    public Button btn_yes;
    public Button btn_no;

    public void OnInit(Data_SaveList data_saveList)
    {
        base.OnInit();

        txt_comment.text = $"이미 열려있는 데이터입니다.\n저장하지 않은 채로 다시 여시겠습니까";
        txt_comment.color = GameManager._gm.color_txt_caution;
        btn_yes.onClick.AddListener(() => Complete(data_saveList));
        btn_no.onClick.AddListener(Exit);
    }
    public void Complete(Data_SaveList data_saveList)
    {
        base.Complete();
        GameManager._panel_saveList.LoadData(data_saveList);
        Destroy(backG);
    }

    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
}
