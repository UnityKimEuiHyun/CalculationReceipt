using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Popup_SaveListNameSet : PanelBase
{
    [Header("UI_Inputfield")]
    public TMP_InputField input_dataName;
    [Header("UI_Button")]
    public Button btn_yes;
    public Button btn_no;

    public override void OnInit()
    {
        base.OnInit();
        btn_yes.onClick.AddListener(Complete);
        btn_no.onClick.AddListener(Exit);
        if (GameManager._panel_costMain.LoadData_SaveList != null)
            input_dataName.text = GameManager._panel_costMain.LoadData_SaveList.saveDataName;
    }
    
    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
    public override void Complete()
    {
        base.Complete();
        if (string.IsNullOrEmpty(input_dataName.text))
        {
            var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
            var popup_comment = CreatePanel(asset_comment);
            popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "데이터 이름 없음", "저장할 데이터의 이름을 입력해주세요.");
            return;
        }
        GameManager._panel_costMain.SaveData(input_dataName.text);
        Destroy(backG);
    }

}
