using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_SaveListOverwrite : PanelBase
{
    [Header("UI_Inputfield")]
    public TMP_InputField input_newDataName;
    [Header("UI_ScrollView_Content")]
    public Transform result_content;
    [Header("UI_Button")]
    public Button btn_No;


    public override void OnInit()
    {
        base.OnInit();
        btn_No.onClick.AddListener(Exit);
        SetList();
    }
    public void SetList()
    {
        for (int i = 0; i < loginInfo.Data_saveList.Count; i++)
        {
            var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_saveListOverwrite");
            var prefab = GameObject.Instantiate(asset, result_content);
            var script = prefab.GetComponent<Prefab_DataOverwrite>();
            var txt_saveName = script.btn_saveName.GetComponentInChildren<TMP_Text>();
            txt_saveName.text = loginInfo.Data_saveList[i].saveDataName;
            script.txt_saveTime.text = loginInfo.Data_saveList[i].saveDataTime;
            int index = i;
            script.btn_saveName.onClick.AddListener(() => FinalComment(txt_saveName.text, index));
        }
    }
    public void FinalComment(string saveName, int index)
    {
        if (string.IsNullOrEmpty(input_newDataName.text))
        {
            var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
            var popup_comment = CreatePanel(asset_comment);
            popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "데이터 이름 없음", "저장할 데이터의 이름을 입력해주세요.");
            return;
        }
        var asset = Resources.Load<GameObject>("Prefabs/Popups/popup_Overwrite_FinalCheck");
        var prefab = CreatePanel(asset);
        var script = prefab.GetComponent<Popup_OverwriteFinalCheck>();
        var newName = input_newDataName.text;
        script.OnInit(saveName, index, newName);
    }
    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
    public override void Complete()
    {
        return;
    }
}
